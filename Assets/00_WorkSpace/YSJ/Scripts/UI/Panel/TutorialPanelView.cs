using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialPanelView : YSJ_PanelBaseUI
{
    private enum TutorialEnum
    {
        TutorialLeft_Button,
        TutorialRight_Button,
        Tutorial_Image,
    }

    [SerializeField] private Sprite[] _tutorialSpriteArray;
    private YSJ_UIBinder<TutorialEnum> _binder;

    private int _currentTutorialPage = 0;

    private int _tutorialStartPage = 0;
    private int _tutorialEndPage = 0;

    public override void InitBaseUI()
    {
        base.InitBaseUI();
        _binder = new(this);

        _tutorialEndPage = _tutorialSpriteArray.Length - 1;
        _currentTutorialPage = _tutorialStartPage;

        _binder.GetEvent(TutorialEnum.TutorialLeft_Button).Click += OnPreviousPage;
        _binder.GetEvent(TutorialEnum.TutorialRight_Button).Click += OnNextPage;

        ChangePage();
    }

    private void OnPreviousPage(PointerEventData data)
    {
        _currentTutorialPage = Mathf.Clamp(_currentTutorialPage - 1, _tutorialStartPage, _tutorialEndPage);
        ChangePage();
    }

    private void OnNextPage(PointerEventData data)
    {
        _currentTutorialPage = Mathf.Clamp(_currentTutorialPage + 1, _tutorialStartPage, _tutorialEndPage);
        ChangePage();
    }

    private void ChangePage()
    {
        // 튜토리얼 이미지 변경
        if (_tutorialSpriteArray != null && _tutorialSpriteArray.Length > 0)
        {
            var tutorialImage = _binder.Get<Image>(TutorialEnum.Tutorial_Image);
            if (tutorialImage != null && _currentTutorialPage >= 0 && _currentTutorialPage < _tutorialSpriteArray.Length)
                tutorialImage.sprite = _tutorialSpriteArray[_currentTutorialPage];
        }

        // 좌/우 버튼 객체 캐싱
        var leftButton = _binder.Get(TutorialEnum.TutorialLeft_Button);
        var rightButton = _binder.Get(TutorialEnum.TutorialRight_Button);

        // 버튼 활성 상태 설정
        bool isFirst = _currentTutorialPage <= _tutorialStartPage;
        bool isLast = _currentTutorialPage >= _tutorialEndPage;

        SetActiveIfChanged(leftButton, !isFirst);
        SetActiveIfChanged(rightButton, !isLast);
    }

    // 현재 상태와 다를 때만 SetActive 호출
    private void SetActiveIfChanged(GameObject go, bool shouldBeActive)
    {
        if (go != null && go.activeSelf != shouldBeActive)
            go.SetActive(shouldBeActive);
    }
}
