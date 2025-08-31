using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_ScoreUIManager : MonoBehaviour
    {
        [SerializeField] private MSG_ScoreUI[] _scoreUIs;
        private int _currentIndex = 0;


        private void OnEnable()
        {
            _currentIndex = 0;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }


        public void ShowScore(int score, float waitSecond)
        {
            StartCoroutine(WaitForShowScoreUI(score, waitSecond));
        }

        // 점수 UI가 1초 늦게 뜨면 좋을 것 같다고 하여 기다림
        private IEnumerator WaitForShowScoreUI(int score, float waitSecond)
        {
            yield return new WaitForSeconds(waitSecond);

            YSJ_AudioManager.Instance.PlaySfx(MSG_AudioDict.Get(MSG_AudioClipKey.Score));

            _scoreUIs[_currentIndex].gameObject.SetActive(true);
            _scoreUIs[_currentIndex].PlayUI(score);

            _currentIndex++;
            if (_currentIndex == _scoreUIs.Length)
            {
                _currentIndex = 0;
            }
        }
    }
}
