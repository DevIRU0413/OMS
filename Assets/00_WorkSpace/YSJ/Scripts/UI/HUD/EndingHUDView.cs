using System.Collections;
using System.Collections.Generic;

using MSG;

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
        ChattingContent,
    }

    [SerializeField] private GameObject _chattingPrefab;

    private YSJ_UIBinder<EndingHUDType> uiBinder;

    [SerializeField] private int _chatCount = 10;
    [SerializeField, Min(0.1f)] private float _chatUpdateMinDelay = 1.0f;
    [SerializeField, Range(0.1f, 100.0f)] private float _chatUpdateMaxDelay = 1.0f;

    [SerializeField] private MSG_NPCNameSO _npcNameSO;
    [SerializeField] private MSG_DialogueSO _dialogueSO;

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

        InitChatting();
    }

    private void InitChatting()
    {
        _chattingGOParent = uiBinder.Get(EndingHUDType.ChattingContent);
        int count = _chatCount;

        if (_chattingGOParent == null || _chattingPrefab == null || count == 0) return;

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

        YSJ_ChattingManager.Instance.Cleanup();

        StartCoroutine(CO_Chatting());
    }

    private IEnumerator CO_Chatting()
    {
        float delayTime = _chatUpdateMaxDelay;

        while (true)
        {
            yield return null;

            delayTime -= Time.deltaTime;

            // 시간 기다렸다가 출력
            if (0 >= delayTime)
            {
                // 출력 업데이트
                int dialogueIndex   = Random.Range(0, _dialogueSO.HiDialogue.Count);
                int nameIndex       = Random.Range(0, _npcNameSO.NameList.Count);
                YSJ_ChattingManager.Instance.AddChattingMessage($"{_npcNameSO.NameList[nameIndex]}: {_dialogueSO.HiDialogue[dialogueIndex]}");
                UpdateChattingContext(YSJ_ChattingManager.Instance?.GetChattingMessages());

                Debug.Log($"{_dialogueSO.HiDialogue[dialogueIndex]}");

                // 출력 업데이트 시간 초기화
                if (_chatUpdateMinDelay < _chatUpdateMaxDelay)
                {
                    delayTime = Random.Range(_chatUpdateMinDelay, _chatUpdateMaxDelay);
                    continue;
                }

                delayTime = _chatUpdateMinDelay;
            }
            continue;
        }
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

        if (catState == null) return;
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

        YSJ_ChattingManager.Instance.MaxChattingCount = _chatCount;
        for (int i = 0; i < _chatCount; i++)
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
