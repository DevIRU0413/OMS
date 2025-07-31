using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_CaptureFailedState : MSG_INpcState
    {
        private MSG_CatchableNPC _npc;

        public MSG_CaptureFailedState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }

        public void Enter()
        {
            _npc.PlayFailEffect();
            _npc.DisableInteraction();
        }
        public void Update()
        {
            // 사라지는 로직?
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
