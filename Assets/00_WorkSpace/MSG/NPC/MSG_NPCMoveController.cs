using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;


namespace MSG
{
    public class MSG_NPCMoveController : MonoBehaviour
    {
        [SerializeField] protected MSG_NPCSettings _settings;
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected MSG_NPCDataSO _dataSO;
        [SerializeField] protected MSG_NPCBase _npcBase;

        private float _stateTime;
        private float _currentTime;
        protected bool _isMoving;
        protected Vector2 _moveDirection;
        private int _forceMoveCount;
        private Direction _endReachedDirection;
        protected bool _isStopped = false;


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


        public virtual void Tick()
        {
            if (_isStopped)
            {
                _npcBase.StartIdleAnim();
                return;
            }

            _currentTime += Time.deltaTime;

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

        // TODO: MapData에 LeftNPCEndPoint를 추가하였으나 현재는 사용하지 않아, 플레이어와 NPC의 이동 반경이 같음
        public virtual void ReachEnd(Direction endReachedDirection)
        {
            Debug.Log($"{gameObject.name}이 맵 경계에 부딪힘");

            _forceMoveCount = _npcBase.Settings.ForceMoveCount;
            _endReachedDirection = endReachedDirection;
        }

        // 라이벌 NPC 날아가는 애니메이션 재생용 이동 중지 플래그
        public void StopMovement()
        {
            _isStopped = true;
        }


        protected virtual void Init()
        {
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
    }
}
