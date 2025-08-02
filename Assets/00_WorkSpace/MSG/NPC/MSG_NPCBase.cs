using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public abstract class MSG_NPCBase : MonoBehaviour
    {
        [SerializeField] protected MSG_NPCDataSO _npcData;
        [SerializeField] protected MSG_NPCMoveController _moveController;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected CapsuleCollider2D _collider;
        [SerializeField] protected SpriteRenderer _spriteRenderer;


        public MSG_NPCMoveController NPCMoveController => _moveController;
        // CSV 파싱 기능 개발 전 NPC 작동을 위한 SO 주입
        public MSG_NPCDataSO NPCData => _npcData;

        protected virtual void Awake()
        {
            if (_moveController == null)
            {
                _moveController = GetComponent<MSG_NPCMoveController>();
            }
        }

        protected virtual void OnEnable()
        {
            _npcData = Instantiate(_npcData); // 새로 생성해서 사용
            Init();
        }

        private void Init()
        {
            _npcData.Init();
        }
    }
}
