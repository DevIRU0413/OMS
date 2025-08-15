using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MSG
{
    // 포획 성공 상태
    public class MSG_CapturedState : MSG_INpcState
    {
        private MSG_PlayerLogic _playerLogic;
        private MSG_CatchableNPC _npc;

        public MSG_CapturedState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }

        public void Enter()
        {
            _npc.StartCapturedMovement();
            _npc.PlayCaptureEffect();
            _npc.DisableInteraction();
            _npc.SpawnHeart();
            _npc.AddCatchScore();
            _npc.DespawnRivalWhenWin();
            _npc.StopAnim();

            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            _playerLogic.AddFollower();
            _playerLogic.Animator.Play(MSG_AnimParams.PLAYER_IDLE);

            if (_playerLogic.IsFever) // 피버타임이라면 
            {
                _npc.PrintSuperChatDialogue(); // 슈퍼챗 대사 출력
            }
            else // 아니면
            {
                _npc.PrintFollowDialogue(); // Follow 대사 출력
            }
        }


        #region Unused Methods
        public void Update() { }
        public void Exit() { }
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        public void OnCatchPressed() { }
        public void OnCatchReleased() { }
        #endregion
    }
}
