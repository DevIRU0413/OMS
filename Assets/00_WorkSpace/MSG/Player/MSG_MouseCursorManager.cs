using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public enum LookDirection
    {
        // 플레이어가 바라볼 방향
        UpRight, Right, DownRight,
        DownLeft, Left, UpLeft
    }

    /// <summary>
    /// 마우스 커서를 통해 플레이어를 이동시키고 포획하는 클래스입니다.
    /// </summary>
    public class MSG_MouseCursorManager : MonoBehaviour
    {
        #region Fields and Properties
        [SerializeField] private MSG_UIInstaller _uiInstaller; // 현재는 타겟 UI를 위해서만 남겨져 있음. 이관해야될 듯
        [SerializeField] private LayerMask _npcLayerMask;
        [SerializeField] private MSG_PlayerSettings _playerSettings;

        private MSG_PlayerLogic _playerLogic;
        private MSG_PlayerData _playerData;
        private SpriteRenderer _spriteRenderer;
        private bool _isMoving = false;
        private bool _isDead = false;

        private MSG_ICatchable _currentHoverTarget; // 현재 올려져있는 타겟,
                                                    // 타겟 UI를 표기하기 위해 사용
        private MSG_ICatchable _pressedTarget;      // 현재 누르고 있는 타겟,
                                                    // _currentHoverTarget만 사용하면 누르고 있다가 마우스가 벗어난 후 마우스 클릭을 떼면 _currentHoverTarget가 null이 되어 따로 사용
        private MSG_ICatchable _recentTarget;       // 가장 최근 눌렀던 타겟,
                                                    // 해당 타겟이 경쟁 중인지를 기억하고 경쟁 중이라면 마우스 이동을 금지하기 위해 기억해야 돼서 따로 사용,
                                                    // 마우스 클릭도 안하고 올려져있지도 않으면 위 변수들이 null이 되기에 따로 사용

        /// <summary>
        /// 포획 중인 타겟,
        /// 경쟁 중이지 않을 때에는 마우스 클릭 중에는 HoverExit이 일어나도 타겟이 변하지 않도록, 경쟁 중일 때에는 경쟁이 끝날 때까지 타겟이 변하지 않도록 고정하기 위해 별도로 사용
        /// 현재 누르고 있는 타겟이 있다면 해당 타겟 반환(비경쟁 포획). 없다면, 가장 최근 눌렀던 타겟이 있고, 경쟁 중이라면 해당 타겟을 반환(경쟁 포획)
        /// </summary>
        private MSG_ICatchable CatchingTarget
        {
            get
            {
                if (_pressedTarget != null && (UnityEngine.Object)_pressedTarget != null)
                {
                    return _pressedTarget;
                }

                if (_recentTarget is MSG_CatchableNPC recent &&
                    recent != null &&
                    recent.IsCompeting
                    )
                {
                    return _recentTarget;
                }

                return null;
            }
        }
        private bool IsCatching => CatchingTarget != null; // 경쟁 유무와 상관없이 포획 중이라면 true 반환

        private float _moveDir;
        public float MoveDir => _moveDir;
        public event Action OnDirectionChanged;

        #endregion


        #region Unity Methods

        private void Start()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            _playerData = MSG_PlayerReferenceProvider.Instance.GetPlayerData();
            _spriteRenderer = _playerLogic.PlayerSpriteRenderer;

            if (_uiInstaller == null)
            {
                Debug.LogError("_uiInstaller is NULL");
            }
            else if (_uiInstaller.UIPresenter == null)
            {
                Debug.LogError("_uiInstaller.UIPresenter is NULL");
            }
        }

        private void Update()
        {
            if (_isDead) return; // 플레이어 사망 시 모든 상호작용 중단

            CheckHoverTarget();
            MoveByMousePos();
            LookByMouseDirection();
            HandleClick();
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// 플레이어 사망 시 상호작용 일괄 중단용
        /// </summary>
        public void StopAll()
        {
            _isDead = true;
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// ScreenToWorldPoint의 마우스 위치를 통해 플레이어를 좌우로 이동
        /// </summary>
        private void MoveByMousePos()
        {
            if (_playerLogic.IsFallen) return; // 플레이어가 넘어져있다면 움직임 정지
            if (IsCatching) return;
            // 가장 최근 타겟이 경쟁 중이라면 플레이어 움직임 정지
            if (_recentTarget != null)
            {
                if (_recentTarget is MSG_CatchableNPC catchble && catchble.IsCompeting)
                {
                    return;
                }
            }

            if (_currentHoverTarget != null) return; // 마우스가 포획 가능한 NPC 위에 있을 때는 이동하지 않음

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 playerPos = _playerLogic.transform.position;

            float deltaX = mouseWorldPos.x - playerPos.x;
            float absDelta = Mathf.Abs(deltaX);

            float speed = 0f;
            if (absDelta < 0.5f)
            {
                speed = 0f;
            }
            else if (absDelta < 2f)
            {
                speed = _playerData.WalkMoveSpeed;
            }
            else
            {
                speed = _playerData.RunSpeed;
            }

            _isMoving = speed > 0f;

            if (_moveDir != Mathf.Sign(deltaX))
            {
                OnDirectionChanged?.Invoke();
            }

            _moveDir = Mathf.Sign(deltaX);

            Vector3 move = new Vector3(_moveDir * speed, 0f, 0f);
            float nextX = playerPos.x + move.x * Time.deltaTime; // 다음 움직일 장소 계산
            float clampedX = Mathf.Clamp(nextX, _playerLogic.CurrentMap.LeftEndPoint, _playerLogic.CurrentMap.RightEndPoint); // 맵의 끝 지점을 벗어나지 못하도록 Clamp

            _playerLogic.transform.position = new Vector3(clampedX, playerPos.y, playerPos.z);
        }

        /// <summary>
        /// 마우스가 포획 가능 NPC 위에 있는지 확인 후 해당 NPC에게 알림 및 타겟 UI 호출
        /// </summary>
        private void CheckHoverTarget()
        {
            if (_playerLogic.IsFallen) return; // 플레이어가 넘어졌다면 타겟 찾기 정지

            if (IsCatching) // 포획 중이라면 현재 포획 중인 타겟으로 강제 고정 후 바로 return
            {
                var catching = CatchingTarget;
                if (_currentHoverTarget != catching) // 대상이 바뀌었을 때만 Hover 관련 호출
                {
                    _currentHoverTarget?.OnHoverExit();
                    _currentHoverTarget = catching;
                    _currentHoverTarget?.OnHoverEnter();
                }
                _uiInstaller.UIPresenter.SetTarget(_currentHoverTarget as MSG_CatchableNPC);
                return;
            }


            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            Collider2D hit = Physics2D.OverlapPoint(mousePos2D, _npcLayerMask);

            if (hit != null)
            {
                if (MSG_NPCProvider.TryGetCatchable(hit, out var catchable))
                {
                    _uiInstaller.UIPresenter.SetTarget(catchable); // UI 호출

                    if ((object)_currentHoverTarget != catchable)
                    {
                        _currentHoverTarget?.OnHoverExit();  // 이전 대상 해제
                        _currentHoverTarget = catchable;
                        _currentHoverTarget.OnHoverEnter();  // 새 대상 진입
                    }

                    if ((object)_recentTarget != catchable)  // 가장 최근 타겟을 저장
                    {
                        _recentTarget = catchable;
                    }

                    return;
                }
            }

            _uiInstaller.UIPresenter.SetTarget(null); // UI 숨김

            // 아무것도 안 가리키고 있을 때
            if (_currentHoverTarget != null)
            {
                _currentHoverTarget.OnHoverExit();
                _currentHoverTarget = null;
            }
        }

        /// <summary>
        /// 마우스 클릭 상호작용 핸들러
        /// </summary>
        private void HandleClick()
        {
            if (_playerLogic.IsFallen) return; // 플레이어가 넘어졌다면 클릭 상호작용 정지

            // 마우스 버튼 다운
            if (Input.GetMouseButtonDown(0))
            {
                // 포획 중이면
                if (IsCatching)
                {
                    // 포획 중인 타겟이랑만 상호작용 허용
                    if (_currentHoverTarget == CatchingTarget)
                    {
                        _pressedTarget = _currentHoverTarget;
                        _pressedTarget.OnCatchPressed();
                    }
                    // else는 상호작용 금지
                }
                // 포획 중이 아니면
                else
                {
                    if (_currentHoverTarget != null)
                    {
                        // 타겟 이동 허용
                        _pressedTarget = _currentHoverTarget;
                        _pressedTarget.OnCatchPressed();
                    }
                }
            }

            // 마우스 버튼 업
            if (Input.GetMouseButtonUp(0))
            {
                if (_pressedTarget != null)
                {
                    // 마우스를 뗀 시점이 HoverExit일 수도 있어 _currentHoverTarget이 null일 가능성이 있어 따로 _pressedTarget를 호출
                    _pressedTarget.OnCatchReleased();
                    _pressedTarget = null;
                }
            }

            // 종료된 타겟 정리
            if (_recentTarget == null) _recentTarget = null;
            if (_pressedTarget == null) _pressedTarget = null;
            if (_currentHoverTarget == null) _currentHoverTarget = null;
        }

        // 스프라이트 교체 형식이라 현재는 뒤집으면 안됨
        // 근데 움직이는 애니메이션 재생을 넣고 사용하는 시점에서는 필요한 메서드
        private void FlipSprite(float moveDir)
        {
            if (_spriteRenderer == null) return;
            _spriteRenderer.flipX = moveDir < 0;
        }

        /// <summary>
        /// 마우스 방향을 기준으로 플레이어 스프라이트 교체
        /// </summary>
        private void LookByMouseDirection()
        {
            if (_playerLogic.IsFallen) return; // 플레이어가 넘어졌다면 교체 정지

            if (_isMoving) return; // 움직이고 있을 때에는 교체하면 안됨
            LookDirection dir = GetMouseDirection();

            _spriteRenderer.sprite = _playerSettings._playerStopSprites[(int)dir];
        }

        /// <summary>
        /// 플레이어가 보는 방향을 6방향으로 변환해 반환합니다
        /// </summary>
        private LookDirection GetMouseDirection()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mouseWorldPos - _playerLogic.transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = (450f - angle) % 360f; // 기존 0도가 오른쪽을 바라보던 것에서 +90을 하여 0도가 위를 바라보도록 보정, 0~360 보정

            foreach (var range in _playerSettings.DirectionAngleRanges)
            {
                // startAngle이 endAngle보다 작은 경우 (예: 60~120도)
                if (range.startAngle < range.endAngle)
                {
                    // angle이 시작~끝 사이에 있는지 확인
                    if (angle >= range.startAngle && angle < range.endAngle)
                        return range.direction;
                }
                // startAngle이 endAngle보다 큰 경우 (예: 330~30도)
                else
                {
                    // angle이 330~360도 이거나 0~30도인 경우
                    if (angle >= range.startAngle || angle < range.endAngle)
                        return range.direction;
                }
            }

            // 기본값 반환
            return LookDirection.Right;
        }

        #endregion
    }
}
