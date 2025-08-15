using System.Collections;
using System.Collections.Generic;

using Core.UnityUtil.PoolTool;

using UnityEngine;


namespace MSG
{
    public class MSG_RivalNPC : MSG_NPCBase
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        private bool _isCompeting = false;
        private Coroutine _flyingCO;

        private void Start()
        {
            MSG_NPCProvider.RegisterRival(this, _collider);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_flyingCO != null)
            {
                StopCoroutine(_flyingCO);
                _flyingCO = null;
            }
            _isCompeting = false;
        }

        private void Update()
        {
            if (_isCompeting) return; // 경쟁 중에는 이동 중지
            _moveController.Tick();
        }

        private void OnDestroy()
        {
            MSG_NPCProvider.UnregisterRival(this, _collider);
        }


        #region Animation Methods

        public override void StartIdleAnim()
        {
            PlayIfPossible(MSG_AnimParams.RIVAL_IDLE);
        }

        public override void StartWalkAnim()
        {
            PlayIfPossible(MSG_AnimParams.RIVAL_WALK);
        }

        public override void StartCatchingAnim()
        {
            PlayIfPossible(MSG_AnimParams.RIVAL_CATCHING);
        }

        public override void StartSurprisedAnim()
        {
            PlayIfPossible(MSG_AnimParams.RIVAL_SURPRISED);
        }

        #endregion


        public void StartCompeting(Transform target)
        {
            _isCompeting = true;

            if ((transform.position.x - target.position.x) > 0) // 타겟이 자신(라이벌)보다 왼쪽에 있으면
            {
                _spriteRenderer.flipX = true; // 기본이 오른쪽, flipX = true
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            StartCatchingAnim();
        }

        public void EndCompeting()
        {
            _isCompeting = false;
            StopAnim();
        }

        // 패배 후 사라져야됨
        public void LoseAndDespawn()
        {
            if (_flyingCO != null)
            {
                StopCoroutine(_flyingCO);
                _flyingCO = null;
            }
            _flyingCO = StartCoroutine(FlyingRoutine());
        }

        // 날아가는 애니메이션
        private IEnumerator FlyingRoutine()
        {
            _moveController.StopMovement();

            _collider.enabled = false;
            _rigidbody2D.gravityScale = _settings.GravityScale;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;

            float angle = Random.Range(_settings.MinAngle, _settings.MaxAngle);
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)); // 0도가 위를 바라봄

            float impulse = _settings.ImpulsePower;
            _rigidbody2D.AddForce(dir * impulse, ForceMode2D.Impulse);

            // 회전 필요시 넣기

            yield return new WaitForSeconds(_settings.DespawnTime);

            _collider.enabled = true;
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            PoolManager.Instance.Despawn(this.gameObject);
        }
    }
}
