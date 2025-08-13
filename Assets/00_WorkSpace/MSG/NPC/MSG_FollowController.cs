using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;


namespace MSG
{
    public class MSG_FollowController : MonoBehaviour
    {
        [SerializeField] private MSG_CatchableNPC _catchableNPC;

        // TODO: 아래 변수 전부 setting으로 옮기기
        [SerializeField] private float _npcDistance = 2f;
        [SerializeField] private float _smoothSpeed = 5f;
        [SerializeField] private float _attachSpeed = 50f;
        [SerializeField] private float _slideOffset = 0.2f;
        [SerializeField] private float _slideDuration = 0.05f;

        private SpriteRenderer _spriteRenderer;
        private Transform _anchor;
        private float _direction;
        private int _index;
        private Vector3 _vel;
        private Coroutine _animationCO;


        public void Init(Transform anchor, int index, float direction)
        {
            _spriteRenderer = _catchableNPC.SpriteRenderer;
            _anchor = anchor;
            _index = index;

            SetDirection(direction);

            if (_animationCO != null)
            {
                StopCoroutine(_animationCO);
                _animationCO = null;
            }
            _animationCO = StartCoroutine(AttachCoroutine());
        }


        private void Update()
        {
            if (_anchor == null) return;

            Vector3 targetPos = _anchor.position + new Vector3(_direction * _npcDistance * (_index + 1), 0f, 0f);

            // 부드럽게 따라오기
            transform.position = Vector3.SmoothDamp(
                transform.position, targetPos, ref _vel,
                1f / Mathf.Max(0.0001f, _smoothSpeed)
            );
        }

        private void SetDirection(float direction)
        {
            _direction = direction < 0 ? -1f : 1f;
            _spriteRenderer.flipX = (direction > 0);
        }

        public void OnDirectionChanged(float direction)
        {
            SetDirection(direction);

            if (_animationCO != null)
            {
                StopCoroutine(_animationCO);
                _animationCO = null;
            }
            _animationCO = StartCoroutine(SlideAnimCoroutine());
        }

        private IEnumerator SlideAnimCoroutine()
        {
            float facing = _spriteRenderer.flipX ? -1f : 1f;
            Vector3 original = transform.position;
            transform.position = original + new Vector3(_slideOffset * -facing, 0f, 0f);
            yield return new WaitForSeconds(_slideDuration);
            transform.position = original;
        }

        private IEnumerator AttachCoroutine()
        {
            while (_anchor != null)
            {
                Vector3 end = _anchor.position + new Vector3(_direction * _npcDistance * (_index + 1), 0f, 0f);
                transform.position = Vector3.Lerp(transform.position, end, Time.deltaTime * _attachSpeed);
                if (Vector3.Distance(transform.position, end) < 0.05f) break;
                yield return null;
            }
        }
    }
}
