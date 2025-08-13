using UnityEngine;

public class InGameHUDPresenter
{
    private InGameHUDView view;
    private YSJ_GameManager modelGM;
    private YSJ_ChattingManager modelCM;

    public InGameHUDPresenter(InGameHUDView view, YSJ_GameManager modelGM, YSJ_ChattingManager modelCM)
    {
        this.view = view;
        this.modelGM = modelGM;
        this.modelCM = modelCM;

        view.InitBaseUI();

        view.OnOpenAction -= Bind;
        view.OnOpenAction += Bind;

        view.OnCloseAction -= Unbind;
        view.OnCloseAction += Unbind;

        view.Open();
    }

    public void Bind()
    {
        // GM
        modelGM.OnHealthChanged += view.UpdateHealth;
        modelGM.OnBatteryChanged += view.UpdateBattery;
        modelGM.OnScoreChanged += view.UpdateScore;
        modelGM.OnFollowerChanged += view.UpdateFollowerCount;
        modelGM.OnChangedReachedFloorEnd += view.ShowNextFloorButton;

        // CM
        modelCM.OnChattingMessageAdded += view.UpdateChattingContext;

        // 초기 UI 반영
        view.UpdateHealth(modelGM.CurrentPlayerHealth);
        view.UpdateBattery(modelGM.BatteryPercent);
        view.UpdateScore(modelGM.Score);
        view.UpdateFollowerCount(modelGM.FollowerCount);
        view.ShowNextFloorButton(modelGM.IsFloorEndReached);
    }

    public void Unbind()
    {
        modelGM.OnHealthChanged -= view.UpdateHealth;
        modelGM.OnBatteryChanged -= view.UpdateBattery;
        modelGM.OnScoreChanged -= view.UpdateScore;
        modelGM.OnFollowerChanged -= view.UpdateFollowerCount;
        modelGM.OnChangedReachedFloorEnd -= view.ShowNextFloorButton;

        modelCM.OnChattingMessageAdded -= view.UpdateChattingContext;
    }
}
