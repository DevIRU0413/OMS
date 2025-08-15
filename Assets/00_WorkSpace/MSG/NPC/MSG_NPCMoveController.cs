using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;


namespace MSG
{
    public class MSG_NPCMoveController : MonoBehaviour
    {
        [SerializeField] private MSG_NPCSettings _settings;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private MSG_NPCDataSO _dataSO;
        [SerializeField] private MSG_NPCBase _npcBase;

        private float _stateTime;
        private float _currentTime;
        private bool _isMoving;
        private Vector2 _moveDirection;
        private int _forceMoveCount;
        private Direction _endReachedDirection;
        private bool _isStopped = false;

        // 강제 이동 관련
        private MSG_PlayerLogic _playerLogic;
        private bool _isForcedMove;
        private float _forcedMoveEndTime;
        private Vector2 _forcedDir;


        private void Awake()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            if (_dataSO == null)
            {
                MSG_NPCBase npc = GetComponent<MSG_NPCBase>();
                _dataSO = npc.NPCData;
            }
        }

        private void OnEnable()
        {
            Init();
        }

        private void Start()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            if (_playerLogic != null)
            {
                _playerLogic.OnPlayerDamaged += MoveWhenTriggered;
            }
        }

        private void OnDisable()
        {
            if (_playerLogic != null)
            {
                _playerLogic.OnPlayerDamaged -= MoveWhenTriggered;
            }
        }


        public void Tick()
        {
            if (_isStopped)
            {
                _npcBase.StartIdleAnim();
                return; // 라이벌 패배 시 이동 정지 후 날아가는 애니메이션을 위한 플래그
            }

            _currentTime += Time.deltaTime;

            // 피격 시 강제 이동 우선 처리
            if (_isForcedMove)
            {
                _npcBase.StartWalkAnim();
                float speed = _dataSO.CharWalkSpeed * _settings.ForcedMoveSpeedMultiplier;
                transform.Translate(_forcedDir * Time.deltaTime * speed);

                if (Time.time >= _forcedMoveEndTime)
                {
                    _isForcedMove = false;
                    // 강제 이동이 끝나면 자연스럽게 다음 배회 상태 재추첨
                    _stateTime = 0f;
                    _currentTime = _stateTime; // 바로 SetNewWanderState() 타게
                }
                return; // 배회 로직 건너뜀
            }

            if (_currentTime >= _stateTime)
            {
                SetNewWanderState();
            }

            if (_isMoving)
            {
                _npcBase.StartWalkAnim();
                transform.Translate(_moveDirection * Time.deltaTime * _dataSO.CharWalkSpeed);
            }
            else
            {
                _npcBase.StartIdleAnim();
            }
        }

        public void ReachEnd(Direction endReachedDirection)
        {
            Debug.Log($"{gameObject.name}이 맵 경계에 부딪힘");

            _forceMoveCount = _npcBase.Settings.ForceMoveCount;
            _endReachedDirection = endReachedDirection;

            if (_isForcedMove) // 강제 이동 중에 맵 경계에 닿으면 즉시 복귀 처리
            {
                _isForcedMove = false;
                _isMoving = true;
                _stateTime = 0f;
                _currentTime = 0f;
            }
        }

        // 라이벌 NPC 날아가는 애니메이션 재생용 이동 중지 플래그
        public void StopMovement()
        {
            _isStopped = true;
        }


        private void Init()
        {
            _isStopped = false;
            _isForcedMove = false;
            _isStopped = false;
            _forceMoveCount = 0;
            _currentTime = 0f;
            _moveDirection = Vector2.zero;
        }

        private void SetNewWanderState()
        {
            _isMoving = Random.value < _settings.MoveProbability; // MoveProbability * 100 % 확률로 이동 결정
            _stateTime = Random.Range(_settings.MinDuration, _settings.MaxDuration); // MinDuration ~ MaxDuration 사이의 행동 시간 설정
            _currentTime = 0f;

            if (_isMoving)
            {
                if (_forceMoveCount > 0)
                {
                    _moveDirection = (_endReachedDirection == Direction.LeftUp || _endReachedDirection == Direction.LeftDown) ? Vector2.right : Vector2.left; // 끝에 다달은 반대 방향으로 강제로 이동하도록 설정
                    _forceMoveCount--;
                }
                else
                {
                    _moveDirection = Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left; // 좌우로 랜덤하게 이동
                }

                _spriteRenderer.flipX = _moveDirection == Vector2.left;
            }
            else
            {
                _moveDirection = Vector2.zero;
            }
        }

        // 플레이어와 피격 시 무한 피격을 막기 위해 강제 이동
        // 요구 사항은 바라보는 방향으로 쭉 가는 것
        private void MoveWhenTriggered()
        {
            if (_npcBase is not MSG_DisturbNPC) return; // 방해 NPC에서만 동작

            _forcedDir = _spriteRenderer.flipX ? Vector2.left : Vector2.right; // 현재 NPC가 바라보는 방향 설정

            _isForcedMove = true;
            _forcedMoveEndTime = Time.time + _settings.ForcedMoveDuration;

            // 배회 상태와 충돌하지 않게 즉시 상태 리셋하여 다음 프레임에 새 상태 뽑도록 처리
            _isMoving = true;
            _stateTime = 0f;
        }
    }
}
