using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_PlayerReferenceProvider : MonoBehaviour
    {
        [SerializeField] private MSG_PlayerLogic _playerLogic;
        private MSG_PlayerData _playerData;

        // 우선 DI 패턴 스크립트가 없어서 싱글톤으로 구현
        public static MSG_PlayerReferenceProvider Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public MSG_PlayerLogic GetPlayerLogic()
        {
            return _playerLogic;
        }

        public MSG_PlayerData GetPlayerData()
        {
            if (_playerData == null)
                _playerData = _playerLogic.PlayerData;

            return _playerData;
        }

        // 이건 필요 없을 듯??
        public Transform GetPlayerTransform()
        {
            return _playerLogic.transform;
        }
    }
}
