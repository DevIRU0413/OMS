using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace MSG
{
    [CreateAssetMenu(fileName = "MSG_PlayerData", menuName = "ScriptableObjects/MSG_PlayerData")]
    public class MSG_PlayerData : ScriptableObject
    {
        public const int MaxHP = 100;
        public const int MinHP = 0;

        [SerializeField] private int _startHP;
        private int _currentHP;
        public int CurrentHP
        {
            get
            {
                return _currentHP;
            }
            set
            {
                _currentHP = value;
                OnCurrentHPChanged?.Invoke(_currentHP);
                YSJ_GameManager.Instance.ChangeHealth(_currentHP);
            }
        }
        [field: SerializeField] public float RunSpeed { get; private set; }             // 달리기 속도
        [field: SerializeField] public float WalkMoveSpeed { get; private set; }        // 걷기 속도
        [field: SerializeField] public float DebuffedMoveSpeed { get; private set; }    // 피격 시 속도

        public event Action<int> OnCurrentHPChanged; // TODO: 필요 없을 수도 있음

        public void Init()
        {
            CurrentHP = _startHP;
            OnCurrentHPChanged?.Invoke(_currentHP);
        }
    }
}
