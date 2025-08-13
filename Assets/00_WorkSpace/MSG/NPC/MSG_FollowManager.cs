using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_FollowManager : MonoBehaviour
    {
        [SerializeField] private MSG_MouseCursorManager _mouseCursorManager;
        
        private Transform _playerTransform;
        private List<MSG_CatchableNPC> _captured = new();
        public List<MSG_CatchableNPC> CapturedList => _captured;

        public static MSG_FollowManager Instance { get; private set; }


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            _mouseCursorManager.OnDirectionChanged += BroadcastDirectionChanged;
            _playerTransform = MSG_PlayerReferenceProvider.Instance.GetPlayerTransform();
        }

        private void OnDestroy()
        {
            if (_mouseCursorManager != null)
            {
                _mouseCursorManager.OnDirectionChanged -= BroadcastDirectionChanged;
            }
        }

        public void AddCapturedNPC(MSG_CatchableNPC npc)
        {
            npc.FollowController.Init(_playerTransform, _captured.Count, _mouseCursorManager.MoveDir);
            _captured.Add(npc);
        }


        private void BroadcastDirectionChanged()
        {
            float moveDir = _mouseCursorManager.MoveDir;

            for (int i = 0; i < _captured.Count; i++)
            {
                _captured[i].FollowController.OnDirectionChanged(moveDir);
            }
        }
    }
}
