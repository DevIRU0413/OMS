using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_FollowController : MonoBehaviour
    {
        [SerializeField] private MSG_CatchableNPC _catchableNPC;
        private SpriteRenderer _spriteRenderer;

        private Transform _targetTransform;
        [SerializeField] private float _moveWeight = 1f;
        [SerializeField] private float _smoothSpeed = 5f;
        [SerializeField] private float _distance = 2f;
        [SerializeField] private float _attachSpeed = 50f;
        [SerializeField] private float _slideOffset = 0.2f;
        [SerializeField] private float _slideDuration = 0.05f;

        private float _dir;
        private int _index;

        public void Init(Transform target, int index, float dir)
        {
            _spriteRenderer = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic().PlayerSpriteRenderer;

            _targetTransform = target;
            _index = index + 1;

            _dir = dir < 0 ? -1f : 1f;
            _spriteRenderer.flipX = _dir > 0;

            StartCoroutine(AttachCoroutine()); // 슬라이드 애니메이션
        }

        private void Update()
        {
            if (_targetTransform == null) return;

            // 이동
            Vector3 offset = new Vector3(_dir * _distance * _index, 0f, 0f);
            Vector3 targetPos = _targetTransform.position + offset;
            //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * _index * _moveWeight);
            if (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * _attachSpeed);
            }
            else
            {
                transform.position = targetPos;
            }
        }

        public void OnDirectionChanged(float dir)
        {
            _dir = dir < 0 ? -1f : 1f;
            _spriteRenderer.flipX = dir > 0;

            StartCoroutine(SlideAnimCoroutine());
        }

        private IEnumerator SlideAnimCoroutine()
        {
            Vector3 offset = new Vector3(_slideOffset * -Mathf.Sign(_spriteRenderer.flipX ? -1 : 1), 0f, 0f);
            Vector3 original = transform.position;
            transform.position += offset;
            yield return new WaitForSeconds(_slideDuration);
            transform.position = original;
        }

        private IEnumerator AttachCoroutine()
        {
            while (true)
            {
                Vector3 end = _targetTransform.position + new Vector3(-_dir * _distance * _index, 0f, 0f);
                transform.position = Vector3.Lerp(transform.position, end, Time.deltaTime * _attachSpeed);

                if (Vector3.Distance(transform.position, end) < 0.1f) break;
                yield return null;
            }
        }
    }
}
