using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_PlayerLogic : MonoBehaviour
    {
        [SerializeField] private MSG_PlayerData _playerData;
        [SerializeField] private Animator _animator;
        [SerializeField] private CapsuleCollider2D _capsuleCollider2D;

        private bool _isWornBreadBag = false;
        private bool _isFever = false;

        public MSG_PlayerData PlayerData => _playerData;
        public bool IsFever => _isFever;

        public event Action OnPlayerFeverStarted;
        public event Action OnPlayerFeverEnded;
        public event Action OnPlayerDied;


        private void Awake()
        {
            _playerData.Init();
        }

        private void Update()
        {
            LowerHPByTime();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 아이템 처리
            // 장애물 NPC 부딪힘 처리
        }

        private void LowerHPByTime()
        {

        }
    }
}
