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
        private MSG_NPCMoveController _moveController;

        public MSG_WanderingState(MSG_CatchableNPC npc)
        {
            _npc = npc;
            _moveController = _npc.NPCMoveController;
        }

        #region State Methods
        public void Enter()
        {
            _currentTime = 0f;
            // 애니메이션 설정
        }

        public void Update()
        {
            _moveController.Tick();
        }

        public void Exit()
        {
            // 애니메이션 설정
        }


        public void OnHoverEnter() => _npc.ChangeState(new MSG_AimedState(_npc));

        #endregion

        #region Private Methods
        #endregion

        #region Unused Methods
        public void OnCatchPressed() { }
        public void OnCatchReleased() { }
        public void OnHoverExit() { }
        #endregion
    }
}
