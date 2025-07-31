using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_CatchingState : MSG_INpcState
    {
        private MSG_CatchableNPC _npc;

        private Coroutine _waitForCheckRivalCO;

        public MSG_CatchingState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }


        public void Enter()
        {
            _npc.StartCaptureGauge();

            if (_waitForCheckRivalCO != null)
            {
                _npc.StopCoroutine(_waitForCheckRivalCO);
            }
            _waitForCheckRivalCO = _npc.StartCoroutine(WaitAndCheckRival());
        }

        public void Update()
        {
            _npc.IncreaseGauge(_npc.Settings.CaptureGaugeIncreasePerSecond * Time.deltaTime);

            if (_npc.IsGaugeFull())
            {
                _npc.ChangeState(new MSG_CapturedState(_npc));
            }
        }

        public void Exit()
        {
            _npc.StopCaptureGauge();
        }

        public void OnCatchReleased()
        {
            _npc.ChangeState(new MSG_AimedState(_npc));
        }

        private IEnumerator WaitAndCheckRival()
        {
            yield return new WaitForSeconds(_npc.Settings.StartDetectionDelay); // 시작 지연 시간 후에 경쟁자 확인
            if (_npc.HasNearbyRival())
            {
                _npc.ChangeState(new MSG_CompetingState(_npc)); // 경쟁 상태로 전환
            }
        }

        #region Unused Methods
        public void OnCatchPressed() { }
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        #endregion
    }

}
