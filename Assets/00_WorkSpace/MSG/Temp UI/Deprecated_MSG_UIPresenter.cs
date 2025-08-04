using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    //public class MSG_UIPresenter
    //{
    //    private MSG_CatchGauge _catchGaugeView;
    //    private MSG_CompeteGauge _competeGaugeView;
    //    private MSG_HPView _hpView;
    //    private MSG_CatchableNPC _npcModel;
    //    private MSG_PlayerLogic _playerModel;

    //    public MSG_UIPresenter(MSG_CatchGauge catchView, MSG_CompeteGauge competeView, MSG_HPView hpView, MSG_CatchableNPC model)
    //    {
    //        _catchGaugeView = catchView;
    //        _competeGaugeView = competeView;
    //        _hpView = hpView;
    //        _npcModel = model;

    //        _npcModel.NPCData.OnGaugeChanged += UpdateGauge;
    //        _npcModel.OnCaptureStarted += () => _catchGaugeView.gameObject.SetActive(true);
    //        _npcModel.OnCaptureEnded += () => _catchGaugeView.gameObject.SetActive(false);
    //        _npcModel.OnCompeteStarted += () => _competeGaugeView.gameObject.SetActive(true);
    //        _npcModel.OnCompeteEnded += () => _competeGaugeView.gameObject.SetActive(false);

    //        _playerModel = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
    //        _playerModel.PlayerData.OnCurrentHPChanged += UpdateHpUI;
    //    }

    //    public void UpdateGauge(float value)
    //    {
    //        float fillAmount = value / _npcModel.NPCData.CharMaxCatchGauge;
    //        _catchGaugeView.SetGauge(fillAmount);
    //        _competeGaugeView.SetGauge(fillAmount);
    //    }

    //    public void UpdateHpUI(int value)
    //    {
    //        _hpView.SetHpUI(value);
    //    }
    //}
}
