using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

using static InGameHUDView;

public class EndingHUDView : YSJ_HUDBaseUI
{
    public enum EndingHUDType
    {
        ToPlayTitle_Button,
        TotalSocre_TMP,
        TotalSocreBG_TMP,
        EndingStateCount_TMP,
        ChattingContent,
    }

    [SerializeField] private GameObject _chattingPrefab;

    private YSJ_UIBinder<EndingHUDType> uiBinder;

    private GameObject _chattingGOParent;                   // chatting parent
    private GameObject[] _chattingGOArray;                  // chatting Log Game Object Array
    private TextMeshProUGUI[] _chattingContentTMPArray;     // chatting Log TMP Array

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


        _chattingGOParent = uiBinder.Get(EndingHUDType.ChattingContent);
        int count = YSJ_ChattingManager.Instance.MaxChattingCount;

        if (_chattingPrefab == null || count == 0) return;

        _chattingGOArray = new GameObject[count];
        _chattingContentTMPArray = new TextMeshProUGUI[count];
        List<TextMeshProUGUI> tmps = new List<TextMeshProUGUI>();
        for (int i = 0; i < count; i++)
        {
            _chattingGOArray[i] = GameObject.Instantiate(_chattingPrefab, _chattingGOParent.transform);
            TextMeshProUGUI tmp = _chattingGOArray[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp) tmps.Add(tmp);
        }
        tmps.Reverse();
        _chattingContentTMPArray = tmps.ToArray();
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

    public void UpdateChattingContext(string[] message)
    {
        if (_chattingContentTMPArray == null)
            _chattingGOParent = uiBinder.Get(EndingHUDType.ChattingContent);

        if (_chattingContentTMPArray == null)
        {
            Debug.Log("Chatting Error");
            return;
        }

        int count = YSJ_ChattingManager.Instance.MaxChattingCount;
        for (int i = 0; i < count; i++)
        {
            var tmp = _chattingContentTMPArray[i];
            if (tmp != null &&
                message != null &&
                message.Length > i)
            {
                tmp.text = message[i];
            }
            else
                tmp.text = string.Empty;
        }
    }
}
