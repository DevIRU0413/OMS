using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace MSG
{
    // Model을 SO로 해도 되나?
    [CreateAssetMenu(fileName = "MSG_PlayerData", menuName = "ScriptableObjects/MSG_PlayerData")]
    public class MSG_PlayerData : ScriptableObject
    {
        public const int MaxHP = 100;
        public const int MinHP = 0;

        [SerializeField] private int _startHP;
        public int CurrentHP { get; private set; }
        [field: SerializeField] public float RunSpeed { get; private set; }
        [field: SerializeField] public float WalkMoveSpeed { get; private set; }
        [field: SerializeField] public float DebuffedMoveSpeed { get; private set; }
        [field: SerializeField] public float HPDecreasePerSecond { get; private set; }

        public event Action<int> OnCurrentHPChanged;

        public void Init()
        {
            CurrentHP = _startHP;
            OnCurrentHPChanged?.Invoke(CurrentHP);
        }
    }
}
