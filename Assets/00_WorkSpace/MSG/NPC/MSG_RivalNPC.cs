using System.Collections;
using System.Collections.Generic;
using Core.UnityUtil.PoolTool;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace MSG
{
    public class MSG_RivalNPC : MSG_NPCBase
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private AnimationClip _surprisedAnimationClip;

        private bool _isCompeteStarted = false;
        private bool _isCompeting = false;
        private Coroutine _flyingCO;
        private Coroutine _animCO;

        public bool IsCompeting => _isCompeting; // 놀란 상태가 아닌 실제 경쟁 시작하였는지 판단


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
            if (_animCO != null)
            {
                StopCoroutine(_animCO);
                _animCO = null;
            }

            _isCompeteStarted = false;
            _isCompeting = false;

            YSJ_GameManager.Instance.OnChangedOver += EndCompeting;
        }

        private void Update()
        {
            if (_isCompeteStarted) return; // 경쟁 중에는 이동 중지
            _moveController.Tick();
        }

        private void OnDestroy()
        {
            MSG_NPCProvider.UnregisterRival(this, _collider);

            if (YSJ_GameManager.Instance != null)
            {
                YSJ_GameManager.Instance.OnChangedOver -= EndCompeting;
            }
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
            if (!IsOnScreen()) return; // 화면 밖에 있으면 return

            if (_isCompeteStarted) return; // 이미 경쟁 중이면 return

            _isCompeteStarted = true;
            _isCompeting = false;

            if ((transform.position.x - target.position.x) > 0) // 타겟이 자신(라이벌)보다 왼쪽에 있으면
            {
                _spriteRenderer.flipX = true; // 기본이 오른쪽, flipX = true
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            if (_animCO != null)
            {
                StopCoroutine(_animCO);
                _animCO = null;
            }
            _animCO = StartCoroutine(CompeteAfterSurprising());
        }

        public void EndCompeting()
        {
            _isCompeteStarted = false;

            if (_animCO != null)
            {
                StopCoroutine(_animCO);
                _animCO = null;
            }

            ForceStartAnim(MSG_AnimParams.RIVAL_IDLE);
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

        // 놀라는 애니메이션 직후 경쟁 실행용 메서드
        private IEnumerator CompeteAfterSurprising()
        {
            StartSurprisedAnim();
            yield return new WaitForSeconds(_surprisedAnimationClip.length);
            ForceStartAnim(MSG_AnimParams.RIVAL_CATCHING);
            _isCompeting = true;
        }

        // 자신(라이벌)이 화면 밖에 있는지 검사하는 메서드
        private bool IsOnScreen()
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);

            // 뷰포트 0~1 사이에 들어오는지 확인
            bool isOnScreen = viewportPos.x >= 0f && viewportPos.x <= 1f &&
                              viewportPos.y >= 0f && viewportPos.y <= 1f;

            return isOnScreen;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var cam = Camera.main;

            // Viewport 좌표
            Vector3 vp = cam.WorldToViewportPoint(transform.position);
            Handles.color = Color.white;
            Handles.Label(transform.position + Vector3.up * 0.3f, $"VP ({vp.x:F2},{vp.y:F2},{vp.z:F2})");
        }
#endif
    }
}
