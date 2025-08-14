using TMPro;

using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms.Impl;

public class EndingHUDView : YSJ_HUDBaseUI
{
    public enum EndingHUDType
    {
        ToPlayTitle_Button,
        TotalSocre_TMP,
        EndingStateCount_TMP,
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

    public void UpdateEndingState(EndingBranchScoreCatData so, int score)
    {
        var catState = uiBinder.Get<TextMeshProUGUI>(EndingHUDType.EndingStateCount_TMP);
        if (so == null)
        {
            catState.text = "-3";
            return;
        }

        catState.text = "-2";
        for (int i = 0; i < so.endingBranchScores.Count; i++)
        {
            if (so.endingBranchScores[i].ScoreCat < score)
            {
                catState.text = i.ToString();
                return;
            }
        }

        catState.text = "-1";
    }
}
