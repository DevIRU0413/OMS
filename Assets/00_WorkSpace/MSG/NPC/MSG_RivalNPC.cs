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


        public void StartCompeting()
        {
            _isCompeting = true;
            // TODO: 바라보는 로직 추가
        }

        public void EndCompeting()
        {
            _isCompeting = false;
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
            // TODO: 매직 넘버 전부 setting으로 옮기기

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
