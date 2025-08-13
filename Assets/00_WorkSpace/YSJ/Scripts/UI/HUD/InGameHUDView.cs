using TMPro;

using UnityEngine;
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

        ChattingUI,

        ChattingContent,
    }

    [SerializeField] private GameObject _chattingPrefab;

    private YSJ_UIBinder<InGameHUDType> uiBinder;

    private GameObject _chattingGOParent;                   // chatting parent
    private GameObject[] _chattingGOArray;                  // chatting Log Game Object Array
    private TextMeshProUGUI[] _chattingContentTMPArray;     // chatting Log TMP Array

    public override void InitBaseUI()
    {
        base.InitBaseUI();
        uiBinder = new(this);

        _chattingGOParent = uiBinder.Get(InGameHUDType.ChattingContent);
        int count = YSJ_ChattingManager.Instance.MaxChattingCount;

        if (_chattingPrefab == null || count == 0) return;

        _chattingGOArray = new GameObject[count];
        _chattingContentTMPArray = new TextMeshProUGUI[count];

        for (int i = 0; i < count; i++)
        {
            _chattingGOArray[i] = GameObject.Instantiate(_chattingPrefab, _chattingGOParent.transform);
            TextMeshProUGUI tmp = _chattingGOArray[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp) _chattingContentTMPArray[i] = tmp;
        }
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

    public void UpdateChattingContext(string[] message)
    {
        if (_chattingContentTMPArray == null)
            _chattingGOParent = uiBinder.Get(InGameHUDType.ChattingContent);

        if (_chattingContentTMPArray == null)
        {
            Debug.Log("Chatting Error");
            return;
        }

        int count = YSJ_ChattingManager.Instance.MaxChattingCount;
        for (int i = 0; i < count; i++)
        {
            TextMeshProUGUI tmp = _chattingGOArray[i].GetComponentInChildren<TextMeshProUGUI>();
            if (tmp) tmp.text = message[i];
        }
    }
}
