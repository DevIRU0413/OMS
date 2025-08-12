using TMPro;

using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class InGameHUDView : YSJ_HUDBaseUI
{
    public enum InGameHUDType
    {
        BatteryCount_Text,
        ScoreCount_Text,
        FollwerCount_Text,
        Health_Slider,
        FloorUI,
    }

    YSJ_UIBinder<InGameHUDType> uiBinder;

    public override void InitBaseUI()
    {
        base.InitBaseUI();
        uiBinder = new(this);
    }

    public void UpdateBattery(float percent)
    {
        uiBinder.Get<TextMeshProUGUI>(InGameHUDType.BatteryCount_Text).text = Mathf.Round(percent).ToString();
    }

    public void UpdateScore(int score)
    {
        uiBinder.Get<TextMeshProUGUI>(InGameHUDType.ScoreCount_Text).text = score.ToString();
    }

    public void UpdateFollowerCount(int count)
    {
        uiBinder.Get<TextMeshProUGUI>(InGameHUDType.FollwerCount_Text).text = count.ToString();
    }

    public void UpdateHealth(int health)
    {
        uiBinder.Get<Slider>(InGameHUDType.Health_Slider).value = health / 100f;
    }

    public void ShowNextFloorButton(bool visible)
    {
        uiBinder.Get(InGameHUDType.FloorUI).SetActive(visible);
    }
}
