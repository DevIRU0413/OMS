using UnityEngine;

public class InGameHUDPresenter
{
    private InGameHUDView view;
    private YSJ_GameManager model;

    public InGameHUDPresenter(InGameHUDView view, YSJ_GameManager model)
    {
        this.view = view;
        this.model = model;

        view.InitBaseUI();
        view.OnOpenAction -= Bind;
        view.OnCloseAction -= Unbind;
        view.OnOpenAction += Bind;
        view.OnCloseAction += Unbind;

        view.Open();
    }

    public void Bind()
    {
        model.OnHealthChanged += view.UpdateHealth;
        model.OnBatteryChanged += view.UpdateBattery;
        model.OnScoreChanged += view.UpdateScore;
        model.OnFollowerChanged += view.UpdateFollowerCount;
        model.OnChangedReachedFloorEnd += view.ShowNextFloorButton;

        // 초기 UI 반영
        view.UpdateHealth(model.CurrentPlayerHealth);
        view.UpdateBattery(model.BatteryPercent);
        view.UpdateScore(model.Score);
        view.UpdateFollowerCount(model.FollowerCount);
        view.ShowNextFloorButton(model.IsFloorEndReached);
    }

    public void Unbind()
    {
        model.OnHealthChanged -= view.UpdateHealth;
        model.OnBatteryChanged -= view.UpdateBattery;
        model.OnScoreChanged -= view.UpdateScore;
        model.OnFollowerChanged -= view.UpdateFollowerCount;
        model.OnChangedReachedFloorEnd -= view.ShowNextFloorButton;
    }
}
