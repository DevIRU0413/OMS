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
        [SerializeField] protected MSG_NPCSettings _settings;
        [SerializeField] protected MSG_FollowController _followController;


        public MSG_NPCMoveController NPCMoveController => _moveController;
        // CSV 파싱 기능 개발 전 NPC 작동을 위한 SO 주입
        public MSG_NPCDataSO NPCData => _npcData;
        public MSG_NPCSettings Settings => _settings;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public MSG_FollowController FollowController
        {
            get
            {
                if (_followController != null)
                {
                    return _followController;
                }
                else
                {
                    Debug.LogWarning("_followController가 없는 NPC에게 요청하거나 _followController가 등록되지 않았습니다.");
                    return null;
                }
            }
        }

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

        // 애니메이션 호출용 가상 메서드
        public virtual void StartIdleAnim() { }
        public virtual void StartWalkAnim() { }
        public virtual void StartCatchingAnim() { }
        public virtual void StartSurprisedAnim() { }
        public void StopAnim()
        {
            _animator.enabled = false;
        }

        // GetCurrentAnimatorStateInfo 한 번만 하기 위해 묶음
        // 또한 특정 상태에서 애니메이션이 덮이지 않게 하기 위함
        protected void PlayIfPossible(int stateHash)
        {
            int current = _animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
            if (current == stateHash ||                         // 현재 이미 동일한 애니메이션 재생 중이거나
                current == MSG_AnimParams.RIVAL_CATCHING ||     // 라이벌 경쟁 중이거나
                current == MSG_AnimParams.RIVAL_SURPRISED ||    // 라이벌 놀란 상태이거나
                current == MSG_AnimParams.CATCHABLE_CATCHING)   // 캐쳐블 포획 중일 때 애니메이션 변경 금지
                return;

            _animator.Play(stateHash);
        }

        // 위 조건의 상태(경쟁 등)에서도 애니메이션을 강제 전환하기 위한 메서드
        public void ForceStartAnim(int stateHash)
        {
            _animator.Play(stateHash);
        }

        private void Init()
        {
            _npcData.Init();
            _animator.enabled = true;
        }
    }
}
