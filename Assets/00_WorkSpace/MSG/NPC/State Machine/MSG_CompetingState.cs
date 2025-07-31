using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_CompetingState : MSG_INpcState
    {
        private MSG_CatchableNPC _npc;
        public MSG_CompetingState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }

        public void Enter()
        {
            _npc.StartCompete();
            _npc.StopMovement(true);
        }


        public void Update()
        {
            _npc.DecreaseGauge(Time.deltaTime * _npc.TotalHealPointPerSecond);

            if (_npc.IsGaugeEmpty())
            {
                _npc.ChangeState(new MSG_CatchingState(_npc));
            }
            else if (_npc.IsGaugeFull())
            {
                _npc.ChangeState(new MSG_CapturedState(_npc));
            }
        }

        public void Exit()
        {
            _npc.EndCompete();
            _npc.StopMovement(false);
        }
        public void OnCatchPressed() 
        {
            _npc.IncreaseGauge(_npc.Settings.CaptureGaugeIncreasePerClick); 
        }

        #region Unused Methods

        public void OnCatchReleased() { }
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        #endregion
    }
}
