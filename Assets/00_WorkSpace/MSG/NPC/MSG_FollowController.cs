using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;


namespace MSG
{
    public class MSG_FollowController : MonoBehaviour
    {
        [SerializeField] private MSG_CatchableNPC _catchableNPC;

        private SpriteRenderer _spriteRenderer;
        private Transform _anchor;
        private float _direction;
        private int _index;
        private Vector3 _vel;
        //private Coroutine _animationCO;
        private bool _isAnimating = false;


        public void Init(Transform anchor, int index, float direction)
        {
            _spriteRenderer = _catchableNPC.SpriteRenderer;
            _anchor = anchor;
            _index = index;

            SetDirection(direction);

            //if (_animationCO != null)
            //{
            //    StopCoroutine(_animationCO);
            //    _animationCO = null;
            //}
            //_animationCO = StartCoroutine(AttachCoroutine());
        }


        private void Update()
        {
            if (_anchor == null) return;
            if (_isAnimating) return; // 애니메이션 중 return

            Vector3 targetPos = _anchor.position + new Vector3(_direction * _catchableNPC.Settings.NpcDistance * (_index + 1), 0f, 0f);

            // 부드럽게 따라오기
            transform.position = Vector3.SmoothDamp(
                transform.position, targetPos, ref _vel,
                1f / Mathf.Max(0.0001f, _catchableNPC.Settings.SmoothSpeed)
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
        }

        //private IEnumerator AttachCoroutine()
        //{
        //    _isAnimating = true;

        //    while (_anchor != null)
        //    {
        //        Vector3 end = _anchor.position + new Vector3(-_direction * _catchableNPC.Settings.NpcDistance * (_index + 1), 0f, 0f);
        //        transform.position = Vector2.Lerp(transform.position, end, Time.deltaTime * _catchableNPC.Settings.AttachSpeed);
        //        if (Vector2.Distance(transform.position, end) < 0.05f) break;
        //        yield return null;
        //    }

        //    _isAnimating = false;
        //}
    }
}
