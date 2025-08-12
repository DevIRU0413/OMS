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

            Debug.Log("포획 실패 Enter");
        }
        public void Update()
        {

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
