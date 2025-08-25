using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInFadeOutSystemView : YSJ_SystemBaseUI
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.25f;

    public Action OnActionFadeIn;
    public Action OnActionFadeOut;

    public bool IsVisible { get; private set; }   // 현재 보이는 상태
    public bool IsFading { get; private set; }   // 페이드 중 여부

    public override void InitBaseUI()
    {
        base.InitBaseUI();
        if (!canvasGroup) canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        IsVisible = false;
        IsFading = false;
        gameObject.SetActive(false);
    }

    public override void Open() => Show();
    public override void Close() => Hide();

    public void Show()
    {
        gameObject.SetActive(true);
        IsFading = true;
        canvasGroup.DOFade(1f, fadeInDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                IsVisible = true;
                IsFading = false;
            });
    }

    public void Hide()
    {
        IsFading = true;
        canvasGroup.DOFade(0f, fadeOutDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                IsVisible = false;
                IsFading = false;
                gameObject.SetActive(false);
            });
    }

    public IEnumerator CO_FadeInOutAction()
    {
        Show();
        yield return new WaitForSeconds(fadeInDuration + 0.1f);
        OnActionFadeIn?.Invoke();

        Hide();
        yield return new WaitForSeconds(fadeOutDuration + 0.1f);
        OnActionFadeOut?.Invoke();
    }
}
