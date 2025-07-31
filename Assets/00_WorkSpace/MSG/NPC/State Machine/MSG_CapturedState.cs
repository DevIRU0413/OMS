using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MSG
{
    public class MSG_CapturedState : MSG_INpcState
    {
        private MSG_CatchableNPC _npc;

        public MSG_CapturedState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }

        public void Enter()
        {
            _npc.MarkAsCaptured();
            _npc.PlayCaptureEffect();
            _npc.DisableInteraction();
        }
        public void Update()
        {
            // 플레이어 뒤를 따라가게
        }


        #region Unused Methods
        public void Exit() { }
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        public void OnCatchPressed() { }
        public void OnCatchReleased() { }
        #endregion
    }
}
