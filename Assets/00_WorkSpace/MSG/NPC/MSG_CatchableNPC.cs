using Core.UnityUtil.PoolTool;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_CatchableNPC : MSG_NPCBase, MSG_ICatchable
    {
        #region Fields and Properties

        [SerializeField] private GameObject _aimObject;
        [SerializeField] private LayerMask _rivalLayer;
        [SerializeField] private MSG_DialogueSO _dialogueSO;

        private MSG_INpcState _currentState;
        private bool _isCompeting;
        private int _rivalCount;
        private float _totalHealPointPerSecond;
        private bool _isCaught = false;
        private bool _isPressed = false;
        private Coroutine _searchRivalCO;


        private List<MSG_RivalNPC> _competingRivals = new(); // 경쟁 중인 라이벌이 정지 후 정지 해제 명령용 캐싱
        private List<MSG_RivalNPC> _surprisedRivals = new(); // 실제 경쟁 들어가지 않았지만 명령이 들어간 NPC 캐싱

        public bool IsCompeting => _isCompeting;
        public int RivalCount => _rivalCount;
        public float TotalHealPointPerSecond => _totalHealPointPerSecond;
        public int FollowScore => _npcData.FollowScore; // 게임 종료 시 포획되었다면 더할 점수, 배율 X
        public bool IsCaught => _isCaught; // FollowScoreTriggerBox에서 포획 되었는지 검사 후 맞다면 점수를 주기 위한 용도
        public bool IsPressed => _isPressed; // 경쟁 상태로 넘어가기 직전 눌린 상태를 확인하는 플래그 추가

        #endregion

        #region Actions
        // UI 활성화 및 비활성화용 Action
        public event Action OnCaptureStarted;   // 경쟁 중이지 않은, 포획 시작 시
        public event Action OnCaptureEnded;     // 경쟁 중이지 않은, 포획 종료 시
        public event Action OnCompeteStarted;   // 경쟁 중일 때,     포획 시작 시
        public event Action OnCompeteEnded;     // 경쟁 중일 때,     포획 종료 시

        #endregion


        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();

            Init();
        }

        private void Start()
        {
            MSG_NPCProvider.RegisterCatchable(this, _collider);
            ChangeState(new MSG_WanderingState(this));
        }

        private void OnDestroy()
        {
            MSG_NPCProvider.UnregisterCatchable(this, _collider);
        }

        private void Update()
        {
            _currentState?.Update();
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 center = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, _settings.DetectionSize);
        }

        #endregion


        #region Animation Methods

        public override void StartIdleAnim()
        {
            PlayIfPossible(MSG_AnimParams.CATCHABLE_IDLE);
        }

        public override void StartWalkAnim()
        {
            PlayIfPossible(MSG_AnimParams.CATCHABLE_WALK);
        }

        public override void StartCatchingAnim()
        {
            PlayIfPossible(MSG_AnimParams.CATCHABLE_CATCHING);
        }

        public override void StartSurprisedAnim() { }

        #endregion

        #region State Management

        public void ChangeState(MSG_INpcState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        #endregion


        #region State Actions
        /// <summary>
        /// 포획 게이지 상승
        /// </summary>
        /// <param name="amount">Update에서 호출 시 초당 올릴 양 * Time.deltaTime / 클릭으로 한 번만 호출 시 한 번에 올릴 양</param>
        public void IncreaseGauge(float amount)
        {
            _npcData.CurrentCharCatchGauge = _npcData.CurrentCharCatchGauge + amount; // IsGaugeFull을 Update에서 검사해서 경쟁 중에 100을 찍었더라도 다시 내려올 수 있음. 그래서 Mathf.Clamp를 사용하지 않음
        }

        /// <summary>
        /// 포획 게이지 하락
        /// </summary>
        /// <param name="amout">초당 내릴 양 * Time.deltaTime</param>
        public void DecreaseGauge(float amout)
        {
            _npcData.CurrentCharCatchGauge = _npcData.CurrentCharCatchGauge - amout; // IsGaugeEmpty를 Update에서 검사해서 경쟁 중에 0을 찍었더라도 다시 올라갈 수 있음. 그래서 Mathf.Clamp를 사용하지 않음
        }

        public bool IsGaugeFull()
        {
            return _npcData.CurrentCharCatchGauge >= _npcData.CharMaxCatchGauge;
        }

        public bool IsGaugeEmpty()
        {
            return _npcData.CurrentCharCatchGauge <= 0f;
        }

        /// <summary>
        /// 경쟁 중이지 않으면서 포획을 시작할 때 호출,
        /// UI 게이지 보이기 시작함
        /// </summary>
        public void StartCaptureGauge()
        {
            // 게이지 UI 시작, 애니메이션 등
            OnCaptureStarted?.Invoke();
            Debug.Log("게이지 증가 시작");
        }

        /// <summary>
        /// 경쟁 중이지 않으면서 포획을 끝낼 때 호출,
        /// UI 게이지 없어짐
        /// </summary>
        public void StopCaptureGauge()
        {
            // 게이지 UI 종료
            OnCaptureEnded?.Invoke();
            Debug.Log("게이지 증가 종료");
        }

        /// <summary>
        /// 경쟁 시작 시 RivalNPC의 포획 게이지 힐 값에 따라 초당 감소량을 계산
        /// </summary>
        public void StartCompete()
        {
            _isCompeting = true;
            OnCompeteStarted?.Invoke();

            if (_searchRivalCO != null)
            {
                StopCoroutine(_searchRivalCO);
                _searchRivalCO = null;
            }
            _searchRivalCO = StartCoroutine(ContinuouslySearchRivalRoutine());
        }

        public void EndCompete()
        {
            _isCompeting = false;
            OnCompeteEnded?.Invoke();

            if (_searchRivalCO != null)
            {
                StopCoroutine(_searchRivalCO);
                _searchRivalCO = null;
            }

            foreach (var rival in _competingRivals) // 경쟁 중 라이벌들 정지 해제
            {
                rival.EndCompeting();
            }
            foreach (var rival in _surprisedRivals) // 경쟁에 아직 들어가진 않았지만 놀란 NPC도 정지 해제
            {
                rival.EndCompeting();
            }
        }

        public void PlayFailEffect()
        {
            // 실패 이펙트
            Debug.Log("포획 실패 효과 실행");
            PoolManager.Instance.Despawn(this.gameObject);
        }

        public void PlayCaptureEffect()
        {
            // 성공 이펙트
            _isCaught = true;
            Debug.Log("포획 성공 효과 실행");
        }

        /// <summary>
        /// 게임 종료 시 호출될 예정입니다.
        /// Follow Score 점수 합산을 위해 콜라이더를 활성화합니다.
        /// </summary>
        public void EnableInteraction()
        {
            _collider.enabled = true;
        }

        public void DisableInteraction()
        {
            // 입력 막기, 충돌 비활성화 등
            _collider.enabled = false;
            Debug.Log("NPC 상호작용 종료");
        }

        /// <summary>
        /// 추종 로직 시작
        /// </summary>
        public void StartCapturedMovement()
        {
            MSG_FollowManager.Instance.AddCapturedNPC(this);
            Debug.Log("NPC 상태: Captured 처리됨");
        }

        public void DespawnRivalWhenWin()
        {
            foreach (var rival in _competingRivals) // 경쟁 중 라이벌들 디스폰
            {
                rival.LoseAndDespawn();
            }
            foreach (var rival in _surprisedRivals) // 놀란 라이벌도 디스폰
            {
                rival.LoseAndDespawn();
            }
        }

        public void ShowAimUI(bool isActive)
        {
            _aimObject.SetActive(isActive);
        }

        public void StopMovement(bool isStopped)
        {
            // idle 애니메이션
        }

        public void SpawnHeart()
        {
            Vector2 spawnPos = new(transform.position.x, transform.position.y - 1f); // 하트는 발 아래에 스폰

            if (_rivalCount >= _settings.XXLargeHeartDropStartRivalCount)
            {
                PoolManager.Instance.Spawn("XXLargeHeartPool", spawnPos, Quaternion.identity);
            }
            else if (_rivalCount >= _settings.XLargeHeartDropStartRivalCount)
            {
                PoolManager.Instance.Spawn("XLargeHeartPool", spawnPos, Quaternion.identity);
            }
            else if (_rivalCount >= _settings.LargeHeartDropStartRivalCount)
            {
                PoolManager.Instance.Spawn("LargeHeartPool", spawnPos, Quaternion.identity);
            }
            else if (_rivalCount >= _settings.MediumHeartDropStartRivalCount)
            {
                PoolManager.Instance.Spawn("MediumHeartPool", spawnPos, Quaternion.identity);
            }
            else if (_rivalCount >= _settings.SmallHeartDropStartRivalCount)
            {
                PoolManager.Instance.Spawn("SmallHeartPool", spawnPos, Quaternion.identity);
            }
        }

        /// <summary>
        /// 근처에 Rival이 있는지 확인
        /// </summary>
        public bool HasNearbyRival()
        {
            Vector2 center = transform.position;
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, _settings.DetectionSize, 0f, _rivalLayer);

            foreach (var col in hits)
            {
                if (MSG_NPCProvider.TryGetRival(col, out var rival))
                {
                    Vector3 viewportPos = Camera.main.WorldToViewportPoint(rival.transform.position);

                    // 뷰포트 0~1 사이에 들어오는지 확인
                    bool isOnScreen = viewportPos.x >= 0f && viewportPos.x <= 1f &&
                                      viewportPos.y >= 0f && viewportPos.y <= 1f;

                    if (!isOnScreen) continue;

                    return true;
                }
            }

            Debug.Log("근처 Rival 없거나 화면 밖에 있음");
            return false;
        }

        /// <summary>
        /// 경쟁 중 클릭 시 오르는 점수
        /// </summary>
        public void AddClickScore()
        {
            //_score += NPCData.ClickScore;
            YSJ_GameManager.Instance.AddScore(RivalCount * 3); // 라이벌 수에 비례하여 배율 적용
        }

        // TODO: 포획 후 주변 라이벌 수와 비례해서 점수 배율 처리 필요한가?
        /// <summary>
        /// 포획 성공 시 오르는 점수
        /// </summary>
        public void AddCatchScore()
        {
            //_score += NPCData.CatchScore;
            YSJ_GameManager.Instance.AddScore(NPCData.CatchScore);
            Debug.Log($"{NPCData.CatchScore}");
        }



        public void PrintHiDialogue()
        {
            int randomIndex = UnityEngine.Random.Range(0, _dialogueSO.HiDialogue.Count);
            YSJ_ChattingManager.Instance.AddChattingMessage(_dialogueSO.HiDialogue[randomIndex]);
            Debug.Log($"{_dialogueSO.HiDialogue[randomIndex]}");
            // UI에 전달하는 메서드
        }

        /// <summary>
        /// 포획 시도 시 출력하는 웃음 대사, 여러 번 나올 수 있습니다
        /// </summary>
        public void PrintLaughDialogue()
        {
            int randomIndex = UnityEngine.Random.Range(0, _dialogueSO.LaughDialogue.Count);
            YSJ_ChattingManager.Instance.AddChattingMessage(_dialogueSO.LaughDialogue[randomIndex]);
            Debug.Log($"{_dialogueSO.LaughDialogue[randomIndex]}");
        }

        /// <summary>
        /// 피버 타임이 아닐 때, 포획 성공 직후 나오는 Follow 대사, 한 번 만 나옵니다
        /// </summary>
        public void PrintFollowDialogue()
        {
            int randomIndex = UnityEngine.Random.Range(0, _dialogueSO.FollowDialogue.Count);
            YSJ_ChattingManager.Instance.AddChattingMessage(_dialogueSO.FollowDialogue[randomIndex]);
            Debug.Log($"{_dialogueSO.FollowDialogue[randomIndex]}");
        }

        /// <summary>
        /// 피버 타임일 때, 포획 성공 직후 나오는 Follow 대사, 한 번 만 나옵니다
        /// </summary>
        public void PrintSuperChatDialogue()
        {
            int randomIndex = UnityEngine.Random.Range(0, _dialogueSO.SuperChatDialogue.Count);
            YSJ_ChattingManager.Instance.AddChattingMessage(_dialogueSO.SuperChatDialogue[randomIndex]);
            Debug.Log($"{_dialogueSO.SuperChatDialogue[randomIndex]}");
        }

        #endregion


        #region Private Methods

        // 활성화 시 콜라이더 등 초기화하는 메서드
        private void Init()
        {
            _isCaught = false;
            _collider.enabled = true;
            _aimObject.SetActive(false);
        }

        // ContinuouslySearchRivalRoutine() 에서 역할을 대신하여 안씀
        /// <summary>
        /// 경쟁 Rival의 포획 게이지 힐 값의 총합을 계산
        /// </summary>
        //private float CalculateRivalPressure()
        //{
        //    _rivalCount = 0;
        //    float total = 0f;
        //    Vector2 center = transform.position;
        //    Collider2D[] hits = Physics2D.OverlapBoxAll(center, _settings.DetectionSize, 0f, _rivalLayer);

        //    _competingRivals.Clear();

        //    foreach (var col in hits)
        //    {
        //        if (MSG_NPCProvider.TryGetRival(col, out var rival))
        //        {
        //            if (!rival.IsCompeting) continue; // 아직 놀란 상태이고 경쟁에는 참여하지 않았다면 continue

        //            _rivalCount++;
        //            total += rival.NPCData.CharCatchGaugeHealValue;
        //            _competingRivals.Add(rival);
        //        }
        //    }

        //    Debug.Log($"경쟁 Rival의 총 포획 게이지 힐 값: {total}");
        //    return total;
        //}

        // 경쟁 중에도 라이벌이 중간 참여하기 위한 메서드
        private IEnumerator ContinuouslySearchRivalRoutine()
        {
            var wait = new WaitForSeconds(0.1f);

            while (true)
            {
                _rivalCount = 0;
                Vector2 center = transform.position;
                Collider2D[] hits = Physics2D.OverlapBoxAll(center, _settings.DetectionSize, 0f, _rivalLayer);

                _competingRivals.Clear();
                _surprisedRivals.Clear();

                float total = 0f;

                foreach (var col in hits)
                {
                    if (MSG_NPCProvider.TryGetRival(col, out var rival))
                    {
                        // 근처 라이벌이면 무조건 경쟁 시작 시도 (라이벌 내부에서 중복 방지)
                        rival.StartCompeting(transform);

                        // 놀람이 끝나 실제 경쟁 상태가 된 라이벌만 압박에 포함
                        if (rival.IsCompeting)
                        {
                            total += rival.NPCData.CharCatchGaugeHealValue;
                            _competingRivals.Add(rival);
                            _rivalCount++;
                        }
                        else
                        {
                            _surprisedRivals.Add(rival);
                        }
                    }
                }

                _totalHealPointPerSecond = total;

                yield return wait;
            }
        }

        #endregion


        #region Callbacks for State
        public void OnHoverEnter() => _currentState?.OnHoverEnter();
        public void OnHoverExit() => _currentState?.OnHoverExit();
        public void OnCatchPressed()
        {
            _isPressed = true;
            _currentState?.OnCatchPressed();
        }
        public void OnCatchReleased()
        {
            _isPressed = false;
            _currentState?.OnCatchReleased();
        }

        #endregion
    }
}
