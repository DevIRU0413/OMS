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


        public void ShowScore(int score)
        {
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
