using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_NPCMoveController : MonoBehaviour
    {
        [SerializeField] private MSG_NPCSettings _settings;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private MSG_NPCDataSO _dataSO;

        private float _stateTime;
        private float _currentTime;
        private bool _isMoving;
        private Vector2 _moveDirection;


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

        public void Tick()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _stateTime)
            {
                SetNewWanderState();
            }

            if (_isMoving)
            {
                transform.Translate(_moveDirection * Time.deltaTime * _dataSO.CharWalkSpeed);
            }
        }

        private void SetNewWanderState()
        {
            _isMoving = Random.value > _settings.MoveProbability; // MoveProbability * 100 % 확률로 이동 결정
            _stateTime = Random.Range(_settings.MinDuration, _settings.MaxDuration); // MinDuration ~ MaxDuration 사이의 행동 시간 설정
            _currentTime = 0f;

            if (_isMoving)
            {
                _moveDirection = Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left; // 좌우로 랜덤하게 이동

                if (_moveDirection == Vector2.left)
                {
                    _spriteRenderer.flipX = _moveDirection == Vector2.left;
                }
            }
            else
            {
                _moveDirection = Vector2.zero;
            }
        }
    }
}
