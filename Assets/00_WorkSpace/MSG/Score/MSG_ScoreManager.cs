using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public static class MSG_ScoreManager
    {
        private static int _score;
        public static int Score => _score;

        public static event Action<int> OnScoreChanged;


        public static void AddScore(int score)
        {
            _score += score;
            OnScoreChanged?.Invoke(_score);
        }


        /// <summary>
        /// 점수 초기화 메서드. 게임 시작 시 호출해야 됩니다
        /// </summary>
        public static void ResetScore()
        {
            _score = 0;
            OnScoreChanged?.Invoke(_score);
        }
    }
}
