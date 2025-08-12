using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_CatchingState : MSG_INpcState
    {
        private MSG_PlayerLogic _playerLogic;
        private MSG_CatchableNPC _npc;
        private Coroutine _waitForCheckRivalCO;

        public MSG_CatchingState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }


        public void Enter()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();

            _npc.StartCaptureGauge();
            _npc.PrintLaughDialogue();

            if (_waitForCheckRivalCO != null)
            {
                _npc.StopCoroutine(_waitForCheckRivalCO);
            }
            _waitForCheckRivalCO = _npc.StartCoroutine(WaitAndCheckRival());
        }

        public void Update()
        {
            _npc.IncreaseGauge(_npc.Settings.CaptureGaugeIncreasePerSecond * Time.deltaTime); // 포획 중 초당 포획 게이지 증가

            _playerLogic.TakeDamage(_playerLogic.PlayerSettings.HPDecreasePerSecond * Time.deltaTime);  // 포획 중 초당 체력 감소

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
