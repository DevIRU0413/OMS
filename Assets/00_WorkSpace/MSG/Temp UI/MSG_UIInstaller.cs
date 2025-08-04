using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_UIInstaller : MonoBehaviour
    {
        [SerializeField] private MSG_CatchGauge _catchGaugeView;
        [SerializeField] private MSG_CompeteGauge _competeGaugeView;
        [SerializeField] private MSG_HPView _hpView;

        private MSG_UIPresenter _uiPresenter;
        public MSG_UIPresenter UIPresenter => _uiPresenter;

        public event Action OnPresenterReady;

        private void Start()
        {
            _uiPresenter = new MSG_UIPresenter(_catchGaugeView, _competeGaugeView, _hpView);
            OnPresenterReady?.Invoke();
        }
    }
}
