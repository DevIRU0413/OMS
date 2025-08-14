using UnityEngine.EventSystems;

public class TitleHUDView : YSJ_HUDBaseUI
{
    private enum TitleEnum
    {
        Start_Button,
        Tutorial_Panel,
    }

    private enum TitleState
    {
        Initial,
        TutorialOpened,
    }

    private YSJ_UIBinder<TitleEnum> _binder;
    private TitleState _currentState = TitleState.Initial;

    public override void InitBaseUI()
    {
        base.InitBaseUI();
        _binder = new(this);

        // 버튼 이벤트 등록
        _binder.GetEvent(TitleEnum.Start_Button).Click += OnClickStart;

        // 튜토리얼 패널 비활성화
        _binder.Get(TitleEnum.Tutorial_Panel)?.SetActive(false);
    }

    private void OnClickStart(PointerEventData data)
    {
        switch (_currentState)
        {
            case TitleState.Initial:
                _binder.Get(TitleEnum.Tutorial_Panel).SetActive(true);
                _currentState = TitleState.TutorialOpened;
                break;

            case TitleState.TutorialOpened:
                // 씬 전환 (YSJ_SystemManager 사용 가정)
                YSJ_SystemManager.Instance.LoadSceneWithPreActions(SceneID.InGameScene.ToString());
                break;
        }
    }
}
