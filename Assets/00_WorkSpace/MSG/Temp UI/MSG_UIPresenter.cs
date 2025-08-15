using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_UIPresenter
    {
        private MSG_CatchGauge _catchGaugeView;
        private MSG_CompeteGauge _competeGaugeView;
        private MSG_HPView _hpView;

        private MSG_PlayerLogic _player;
        private MSG_CatchableNPC _currentTarget;
        public MSG_CatchableNPC CurrentTarget => _currentTarget; // UI가 Targer을 알기 위해 열어둠
        public event Action OnTargetChanged; // UI가 Target이 바뀐 것을 알기 위해 열어둠

        public MSG_UIPresenter(MSG_CatchGauge catchGauge, MSG_CompeteGauge competeGauge, MSG_HPView hpView)
        {
            _catchGaugeView = catchGauge;
            _competeGaugeView = competeGauge;
            _hpView = hpView;

            _player = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            _player.PlayerData.OnCurrentHPChanged += UpdateHpUI;

            if (_player.PlayerData != null)
            {
                Debug.Log($"{_player.PlayerData.CurrentHP}");
                UpdateHpUI(_player.PlayerData.CurrentHP);
            }
            else
            {
                Debug.LogWarning("_player.PlayerData가 null");
            }
        }

        public void SetTarget(MSG_CatchableNPC target)
        {
            // 기존 타겟 구독 해제
            if (_currentTarget != null)
            {
                _currentTarget.NPCData.OnGaugeChanged -= UpdateGaugeUI;
                _currentTarget.OnCaptureStarted -= ShowCatchGauge;
                _currentTarget.OnCaptureEnded -= HideCatchGauge;
                _currentTarget.OnCompeteStarted -= ShowCompeteGauge;
                _currentTarget.OnCompeteEnded -= HideCompeteGauge;
            }

            if (_currentTarget != target)
            {
                _currentTarget = target;
                OnTargetChanged?.Invoke();
            }

            if (_currentTarget != null)
            {
                // 새 타겟 구독
                _currentTarget.NPCData.OnGaugeChanged += UpdateGaugeUI;
                _currentTarget.OnCaptureStarted += ShowCatchGauge;
                _currentTarget.OnCaptureEnded += HideCatchGauge;
                _currentTarget.OnCompeteStarted += ShowCompeteGauge;
                _currentTarget.OnCompeteEnded += HideCompeteGauge;

                // 초기 UI 상태 갱신
                UpdateGaugeUI(_currentTarget.NPCData.CurrentCharCatchGauge);
            }
            else
            {
                HideUIAll();
            }
        }

        private void UpdateGaugeUI(float current)
        {
            // 게이지 위치 NPC 위치 참조하여 변경 로직 추가

            float fill = current / _currentTarget.NPCData.CharMaxCatchGauge;
            _catchGaugeView.SetGauge(fill);
            _competeGaugeView.SetGauge(fill);
        }

        private void UpdateHpUI(int value)
        {
            //_hpView.SetHpUI(value);
        }

        private void ShowCatchGauge()
        {
            _catchGaugeView.gameObject.SetActive(true);
        }

        private void HideCatchGauge()
        {
            _catchGaugeView.gameObject.SetActive(false);
        }

        private void ShowCompeteGauge()
        {
            _competeGaugeView.gameObject.SetActive(true);
        }

        private void HideCompeteGauge()
        {
            _competeGaugeView.gameObject.SetActive(false);
        }

        private void HideUIAll()
        {
            _catchGaugeView.gameObject.SetActive(false);
            _competeGaugeView.gameObject.SetActive(false);
        }
    }
}
