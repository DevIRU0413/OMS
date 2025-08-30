using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_CompetingState : MSG_INpcState
    {
        private MSG_PlayerLogic _playerLogic;
        private MSG_CatchableNPC _npc;

        public MSG_CompetingState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }

        public void Enter()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            _playerLogic.SetActiveFightEffectByNPC(true);

            _npc.StartCompete();
            _npc.StopMovement(true);

            YSJ_GameManager.Instance.OnChangedOver += StopAll;
        }


        public void Update()
        {
            _npc.DecreaseGauge(Time.deltaTime * _npc.TotalHealPointPerSecond);

            if (_npc.IsGaugeEmpty())
            {
                _npc.ChangeState(new MSG_CaptureFailedState(_npc));
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

            if (YSJ_GameManager.Instance != null)
            {
                YSJ_GameManager.Instance.OnChangedOver -= StopAll;
            }
        }

        public void OnCatchPressed() 
        {
            _playerLogic.TakeDamage(_playerLogic.PlayerSettings.HPDecreasePerClick * _npc.RivalCount); // 클릭 당 체력 감소 * 라이벌 수 만큼 플레이어 체력 감소

            if (_playerLogic.IsFever)
            {
                _npc.IncreaseGauge(_npc.Settings.CaptureGaugeIncreasePerClick *
                    _playerLogic.PlayerSettings.FeverGaugeIncreaseMagnifier); // 피버 시 클릭 당 포획 게이지 증가
            }
            else
            {
                _npc.IncreaseGauge(_npc.Settings.CaptureGaugeIncreasePerClick); // 클릭 당 포획 게이지 증가
            }

            _npc.AddClickScore();
        }

        private void StopAll()
        {
            _npc.ChangeState(new MSG_WanderingState(_npc));
            _npc.ForceStartAnim(MSG_AnimParams.CATCHABLE_IDLE);
        }


        #region Unused Methods

        public void OnCatchReleased() { }
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        #endregion
    }
}
