using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSG
{
    public class MSG_WanderingState : MSG_INpcState
    {
        private MSG_CatchableNPC _npc;

        private float _stateTime;
        private float _currentTime;
        private bool _isMoving;
        private Vector2 _moveDirection;

        public MSG_WanderingState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }

        #region State Methods
        public void Enter()
        {
            _currentTime = 0f;
            // 애니메이션 설정
        }

        public void Update()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _stateTime)
            {
                SetNewWanderState();
            }

            if (_isMoving)
            {
                _npc.transform.Translate(_moveDirection * Time.deltaTime * _npc.NPCData.CharWalkSpeed);
            }
        }

        public void Exit()
        {
            // 애니메이션 설정
        }


        public void OnHoverEnter() => _npc.ChangeState(new MSG_AimedState(_npc));

        #endregion

        #region Private Methods

        private void SetNewWanderState()
        {
            _isMoving = Random.value > _npc.Settings.MoveProbability; // MoveProbability * 100 % 확률로 이동 결정
            _stateTime = Random.Range(_npc.Settings.MinDuration, _npc.Settings.MaxDuration); // MinDuration ~ MaxDuration 사이의 행동 시간 설정
            _currentTime = 0f;

            if (_isMoving)
            {
                _moveDirection = Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left; // 좌우로 랜덤하게 이동

                if (_moveDirection == Vector2.left)
                {
                    _npc.FlipX(true);
                }
            }
            else
            {
                _moveDirection = Vector2.zero;
            }
        }

        #endregion

        #region Unused Methods
        public void OnCatchPressed() { }
        public void OnCatchReleased() { }
        public void OnHoverExit() { }
        #endregion
    }
}
