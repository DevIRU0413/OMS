using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MSG
{
    // 마우스가 위로 올라간 상태
    public class MSG_AimedState : MSG_INpcState
    {
        private MSG_PlayerLogic _playerLogic;
        private MSG_CatchableNPC _npc;

        private float _stateTime;
        private float _currentTime;
        private bool _isMoving;
        private Vector2 _moveDirection;
        private MSG_NPCMoveController _controller;

        public MSG_AimedState(MSG_CatchableNPC npc)
        {
            _npc = npc;
            _controller = _npc.NPCMoveController;

            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
        }

        public void Enter()
        {
            _npc.ShowAimUI(true);
            _npc.ForceStartAnim(MSG_AnimParams.CATCHABLE_IDLE);
            _npc.SetActiveEffect(false);
        }

        public void Update()
        {
            _controller.Tick();
        }

        public void Exit()
        {
            _npc.ShowAimUI(false);
            _playerLogic.Animator.Play(MSG_AnimParams.PLAYER_IDLE);
        }


        public void OnHoverExit() => _npc.ChangeState(new MSG_WanderingState(_npc));
        public void OnCatchPressed() => _npc.ChangeState(new MSG_CatchingState(_npc));


        #region Unused Methods
        public void OnHoverEnter() { }
        public void OnCatchReleased() { }
        #endregion
    }
}
