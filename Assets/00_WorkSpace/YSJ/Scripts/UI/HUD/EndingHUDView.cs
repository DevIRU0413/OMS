using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

public class EndingHUDView : YSJ_HUDBaseUI
{
    public enum EndingHUDType
    {
        ToPlayTitle_Button,
        TotalSocre_TMP,
        TotalSocreBG_TMP,
        EndingStateCount_TMP,
    }

    private YSJ_UIBinder<EndingHUDType> uiBinder;

    public override void InitBaseUI()
    {
        base.InitBaseUI();
        uiBinder = new(this);

        Debug.Log("EndUI 초기화");
        uiBinder.GetEvent(EndingHUDType.ToPlayTitle_Button).Click += (PointerEventData eventData) =>
        {
            YSJ_SystemManager.Instance.LoadSceneWithPreActions(SceneID.TitleScene.ToString());
            Debug.Log("타이틀로 이동");
        };
    }

    public void UpdateTotalScore(int score)
    {
        string scoreString = score.ToString();

        var scoreTMP = uiBinder.Get<TextMeshProUGUI>(EndingHUDType.TotalSocre_TMP);
        if (scoreTMP != null)
            scoreTMP.text = scoreString;


        var scoreBgTMP = uiBinder.Get<TextMeshProUGUI>(EndingHUDType.TotalSocreBG_TMP);
        if (scoreBgTMP != null)
            scoreBgTMP.text = scoreString;
    }

    public void UpdateEndingState(EndingBranchScoreCatData so, int score)
    {
        var catState = uiBinder.Get<TextMeshProUGUI>(EndingHUDType.EndingStateCount_TMP);
        if (so == null)
        {
            catState.text = "-2";
            return;
        }

        if (catState == null) return;

        catState.text = "-1";
        for (int i = 0; i < so.endingBranchScores.Count; i++)
        {
            if (so.endingBranchScores[i].ScoreCat <= score)
            {
                catState.text = i.ToString();
            }
        }
    }
}
