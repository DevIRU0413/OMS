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
        [SerializeField] protected Rigidbody2D _rigidbody2D;
        [SerializeField] protected AnimationClip _surprisedAnimationClip;
        [SerializeField] protected GameObject _fightEffectObj;

        // === 추가: 날아갈 때 사용할 전용 스프라이트(개별 NPC별 지정) ===
        [SerializeField] private Sprite _flyingSprite;

        // 내부 캐시(원복용)
        private Sprite _cachedSprite;
        private bool _animatorWasEnabled;

        protected bool _isCompeteStarted = false;
        protected bool _isCompeting = false;
        private Coroutine _flyingCO;
        protected Coroutine _animCO;

        public bool IsCompeting => _isCompeting; // 놀란 상태가 아닌 실제 경쟁 시작하였는지 판단


        protected virtual void Start()
        {
            MSG_NPCProvider.RegisterRival(this, _collider);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            OnEnableInitHooker();
            OnEnableAnimHooker();
        }

        protected virtual void Update()
        {
            if (_isCompeteStarted) return; // 경쟁 중에는 이동 중지
            _moveController.Tick();
        }

        protected virtual void OnDisable()
        {
            if (YSJ_GameManager.Instance != null)
            {
                YSJ_GameManager.Instance.OnChangedOver -= EndCompeting;
            }
        }

        protected virtual void OnDestroy()
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


        public virtual void StartCompeting(Transform target)
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

            YSJ_AudioManager.Instance.PlaySfx(MSG_AudioDict.Get(MSG_AudioClipKey.RivalCompete));
        }

        public virtual void EndCompeting()
        {
            _isCompeteStarted = false;

            if (_animCO != null)
            {
                StopCoroutine(_animCO);
                _animCO = null;
            }

            ForceStartAnim(MSG_AnimParams.RIVAL_IDLE);
            SetActiveFightObj(false);
        }

        // 패배 후 사라져야됨
        public virtual void LoseAndDespawn()
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

            // === 스프라이트 교체 준비 ===
            if (_animator != null)
            {
                _animatorWasEnabled = _animator.enabled;
                _animator.enabled = false; // 애니메이터가 덮어쓰지 않도록 끔
            }

            _cachedSprite = _spriteRenderer.sprite; // 원래 스프라이트 저장
            if (_flyingSprite != null)
            {
                _spriteRenderer.sprite = _flyingSprite;
            }

            // === 기존 물리 연출 ===
            _collider.enabled = false;
            _rigidbody2D.gravityScale = _settings.GravityScale;
            _rigidbody2D.constraints = RigidbodyConstraints2D.None;

            float angle = Random.Range(_settings.MinAngle, _settings.MaxAngle);
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad)); // 0도가 위를 바라봄

            float impulse = _settings.ImpulsePower;
            _rigidbody2D.AddForce(dir * impulse, ForceMode2D.Impulse);

            float originalAngularDrag = _rigidbody2D.angularDrag;
            float sign = Random.value < 0.5f ? 1f : -1f;
            float spin = Random.Range(_settings.MinSpinAngularVel, _settings.MaxSpinAngularVel) * sign;

            _rigidbody2D.angularDrag = _settings.TempAngularDrag;
            _rigidbody2D.angularVelocity = spin;

            yield return new WaitForSeconds(_settings.DespawnTime);

            // === 스프라이트 및 애니메이터 원복 ===
            _spriteRenderer.sprite = _cachedSprite;
            if (_animator != null)
            {
                _animator.enabled = _animatorWasEnabled;
            }

            // === 상태 원복 후 디스폰 ===
            _collider.enabled = true;
            _rigidbody2D.gravityScale = 0f;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            PoolManager.Instance.Despawn(this.gameObject);
        }

        // 놀라는 애니메이션 직후 경쟁 실행용 메서드
        protected virtual IEnumerator CompeteAfterSurprising()
        {
            StartSurprisedAnim();
            yield return new WaitForSeconds(_surprisedAnimationClip.length);
            ForceStartAnim(MSG_AnimParams.RIVAL_CATCHING);
            SetActiveFightObj(true);
            _isCompeting = true;
        }

        // 자신(라이벌)이 화면 밖에 있는지 검사하는 메서드
        protected virtual bool IsOnScreen()
        {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            // 뷰포트 0~1 사이에 들어오는지 확인
            bool isOnScreen = viewportPos.x >= 0f && viewportPos.x <= 0.85f &&
                viewportPos.y >= 0f && viewportPos.y <= 1f;

            return isOnScreen;
        }

        protected virtual void OnEnableInitHooker()
        {
            _isCompeteStarted = false;
            _isCompeting = false;

            YSJ_GameManager.Instance.OnChangedOver += EndCompeting;
        }

        protected virtual void OnEnableAnimHooker()
        {
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

            // 안전 복구
            if (_cachedSprite != null)
            {
                _spriteRenderer.sprite = _cachedSprite;
            }
            if (_animator != null)
            {
                _animator.enabled = true;
            }
        }

        protected void SetActiveFightObj(bool active)
        {
            _fightEffectObj.SetActive(active);
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            var cam = Camera.main;
            Vector3 vp = cam.WorldToViewportPoint(transform.position);
            Handles.color = Color.white;
            Handles.Label(transform.position + Vector3.up * 0.3f, $"VP ({vp.x:F2},{vp.y:F2},{vp.z:F2})");
        }
#endif
    }
}
