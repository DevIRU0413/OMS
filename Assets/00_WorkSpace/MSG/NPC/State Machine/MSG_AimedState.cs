using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MSG
{
    public class MSG_AimedState : MSG_INpcState
    {
        private MSG_CatchableNPC _npc;

        private float _stateTime;
        private float _currentTime;
        private bool _isMoving;
        private Vector2 _moveDirection;

        public MSG_AimedState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }

        public void Enter()
        {
            _npc.ShowAimUI(true);
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
            _npc.ShowAimUI(false);
        }


        public void OnHoverExit() => _npc.ChangeState(new MSG_WanderingState(_npc));
        public void OnCatchPressed() => _npc.ChangeState(new MSG_CatchingState(_npc));


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

        #region Unused Methods
        public void OnHoverEnter() { }
        public void OnCatchReleased() { }
        #endregion
    }
}
