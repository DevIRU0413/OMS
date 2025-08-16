using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_DirectionSender : MonoBehaviour
    {
        [SerializeField] private MSG_FloorButtonManager _floorButtonManager;
        [SerializeField] private MSG_MapChanger _mapChanger;
        [SerializeField] private Direction _direction;

        private MSG_PlayerLogic _playerLogic;
        private bool _isDead = false;


        private void Start()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            _playerLogic.OnPlayerDied += OnPlayerDied;
        }

        private void OnEnable()
        {
            _isDead = false;
        }

        private void OnDisable()
        {
            if (_playerLogic != null)
            {
                _playerLogic.OnPlayerDied -= OnPlayerDied;
            }
        }


        public void OnClickMoveMap()
        {
            if (!_floorButtonManager.TryClickButton()) return;
            if (_isDead) return;

            Debug.Log("OnClickMoveMap 호출");
            _mapChanger.ChangeMap(_direction);
        }


        private void OnPlayerDied()
        {
            _isDead = true;
        }
    }
}
