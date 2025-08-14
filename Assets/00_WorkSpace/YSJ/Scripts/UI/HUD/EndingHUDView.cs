using TMPro;

using UnityEngine.EventSystems;

public class EndingHUDView : YSJ_HUDBaseUI
{
    public enum EndingHUDType
    {
        ToPlayTitle_Button,
        TotalSocre_TMP,
    }

    private YSJ_UIBinder<EndingHUDType> uiBinder;

    public override void InitBaseUI()
    {
        base.InitBaseUI();
        uiBinder = new(this);

        uiBinder.GetEvent(EndingHUDType.ToPlayTitle_Button).Click += (PointerEventData eventData) =>
        {
            YSJ_SystemManager.Instance.LoadSceneWithPreActions(SceneID.TitleScene.ToString());
        };
    }

    public void UpdateTotalScore(int score)
    {
        var scoreTMP = uiBinder.Get<TextMeshProUGUI>(EndingHUDType.TotalSocre_TMP);
        scoreTMP.text = score.ToString();
    }
}
