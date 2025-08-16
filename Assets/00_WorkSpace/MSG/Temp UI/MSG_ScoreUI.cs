using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;


namespace MSG
{
    public class MSG_ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rect;

        [Header("DoTween 설정")]
        [SerializeField] private float _riseDistance = 60f;
        [SerializeField] private float _duration = 0.8f;
        [SerializeField] private Ease _moveEase = Ease.OutQuad;
        [SerializeField] private Ease _fadeEase = Ease.InQuad;

        private Vector2 _originAnchoredPos;
        private Tween _tween;


        private void Awake()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }
        }

        private void OnDisable()
        {
            _tween?.Kill();
            _tween = null;

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
            }
        }

        public void PlayUI(int score)
        {
            _tween?.Kill();
            gameObject.SetActive(true);

            _originAnchoredPos = _rect.anchoredPosition;

            _scoreText.text = score.ToString();
            _canvasGroup.alpha = 1f;

            var seq = DOTween.Sequence();

            seq.Join(_rect.DOAnchorPosY(_originAnchoredPos.y + _riseDistance, _duration)
                .SetEase(_moveEase))
                .Join(_canvasGroup.DOFade(0f, _duration)
                .SetEase(_fadeEase))
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                })
                .SetLink(gameObject);

            _tween = seq;
        }
    }
}
