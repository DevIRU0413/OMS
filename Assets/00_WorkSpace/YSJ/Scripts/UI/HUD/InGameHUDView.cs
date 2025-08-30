using System.Collections.Generic;

using MSG;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
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

        RightUp_Button,
        RightDown_Button,

        LeftUp_Button,
        LeftDown_Button,
    }

    private const int MAP_START_STEP = 1;
    private const int MAP_END_STEP = 4;

    [SerializeField] private GameObject _chattingPrefab;
    [SerializeField] private FadeInFadeOutSystemView fadeInOutSystemView;
    private YSJ_UIBinder<InGameHUDType> uiBinder;
    private ReachedFloorDirection _floorDirection;

    private GameObject _chattingGOParent;                   // chatting parent
    private GameObject[] _chattingGOArray;                  // chatting Log Game Object Array
    private TextMeshProUGUI[] _chattingContentTMPArray;     // chatting Log TMP Array
    private MSG_PlayerLogic _player;

    public override void InitBaseUI()
    {
        base.InitBaseUI();

        uiBinder = new(this);
        _floorDirection = ReachedFloorDirection.None;

        _chattingGOParent = uiBinder.Get(InGameHUDType.ChattingContent);
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

        _player = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();

        if (fadeInOutSystemView != null)
        {
            fadeInOutSystemView.InitBaseUI();
            uiBinder.GetEvent(InGameHUDType.RightUp_Button  ).Click -= FadeInOutAction;
            uiBinder.GetEvent(InGameHUDType.RightDown_Button).Click -= FadeInOutAction;
            uiBinder.GetEvent(InGameHUDType.LeftUp_Button   ).Click -= FadeInOutAction;
            uiBinder.GetEvent(InGameHUDType.LeftDown_Button ).Click -= FadeInOutAction;

            uiBinder.GetEvent(InGameHUDType.RightUp_Button  ).Click += FadeInOutAction;
            uiBinder.GetEvent(InGameHUDType.RightDown_Button).Click += FadeInOutAction;
            uiBinder.GetEvent(InGameHUDType.LeftUp_Button   ).Click += FadeInOutAction;
            uiBinder.GetEvent(InGameHUDType.LeftDown_Button ).Click += FadeInOutAction;
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

    public void ShowNextFloorButton(ReachedFloorDirection direction)
    {
        uiBinder.Get(InGameHUDType.FloorUI).SetActive(direction != ReachedFloorDirection.None);
        int currentFloor = _player.CurrentMap.CurrentFloor;
        int floorType = -1; // 0 최하층, 1 중간, 2 최상층
        print(currentFloor);
        if (currentFloor == MAP_START_STEP)
        {
            floorType = 0;
        }
        else if (currentFloor == MAP_END_STEP)
        {
            floorType = 2;
        }
        else
        {
            floorType = 1;
        }

        if (direction == ReachedFloorDirection.Right)
        {
            switch (floorType)
            {
                case 0:
                    uiBinder.Get(InGameHUDType.RightUp_Button).SetActive(true);
                    uiBinder.Get(InGameHUDType.RightDown_Button).SetActive(false);
                    break;

                case 1:
                    uiBinder.Get(InGameHUDType.RightUp_Button).SetActive(true);
                    uiBinder.Get(InGameHUDType.RightDown_Button).SetActive(true);
                    break;

                default:
                    uiBinder.Get(InGameHUDType.RightUp_Button).SetActive(false);
                    uiBinder.Get(InGameHUDType.RightDown_Button).SetActive(true);
                    break;
            }
        }
        else if (direction == ReachedFloorDirection.Left)
        {
            switch (floorType)
            {
                case 0:
                    uiBinder.Get(InGameHUDType.LeftUp_Button).SetActive(true);
                    uiBinder.Get(InGameHUDType.LeftDown_Button).SetActive(false);
                    break;

                case 1:
                    uiBinder.Get(InGameHUDType.LeftUp_Button).SetActive(true);
                    uiBinder.Get(InGameHUDType.LeftDown_Button).SetActive(true);
                    break;

                default:
                    uiBinder.Get(InGameHUDType.LeftUp_Button).SetActive(false);
                    uiBinder.Get(InGameHUDType.LeftDown_Button).SetActive(true);
                    break;
            }
        }
        else
        {
            uiBinder.Get(InGameHUDType.RightUp_Button).SetActive(false);
            uiBinder.Get(InGameHUDType.RightDown_Button).SetActive(false);

            uiBinder.Get(InGameHUDType.LeftUp_Button).SetActive(false);
            uiBinder.Get(InGameHUDType.LeftDown_Button).SetActive(false);
        }
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

    public void FadeInOutAction(PointerEventData eventData)
    {
        if (fadeInOutSystemView != null)
        {
            StartCoroutine(fadeInOutSystemView.CO_FadeInOutAction());
        }
    }
}
