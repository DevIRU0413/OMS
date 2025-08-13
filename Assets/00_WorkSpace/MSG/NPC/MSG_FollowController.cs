using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace MSG
{
    public class MSG_FollowController : MonoBehaviour
    {
        [SerializeField] private MSG_CatchableNPC _catchableNPC;

        private SpriteRenderer _spriteRenderer;
        private Transform _anchor;
        private float _direction;
        private int _index;

        private float _xVel;
        private float _yVel;
        private float _baseX; // 보블 적용 전 순수 추적 위치 X
        private float _baseY; // 보블 적용 전 순수 추적 위치 Y

        // DoTween 설정
        private Tween _doTween;
        private float _bobOffset;  // 트윈 대상 값, Update에서 최종 Y에 합산용

        public float Direction => _direction; // FollowManager에서 현재 대기 중인 위치를 직접 참조하여 서로 다른 위치에 배치 막기 위함


        private void Update()
        {
            if (_anchor == null) return;

            float smoothTime = 1f / Mathf.Max(0.0001f, _catchableNPC.Settings.SmoothSpeed);

            float targetX = _anchor.position.x + _direction * _catchableNPC.Settings.NpcDistance * (_index + 1);
            float targetY = _anchor.position.y;

            // X, Y 각각 부드럽게 추적
            _baseX = Mathf.SmoothDamp(_baseX, targetX, ref _xVel, smoothTime);
            _baseY = Mathf.SmoothDamp(_baseY, targetY, ref _yVel, smoothTime);

            // bobOffset은 최종 Y에만 합산
            transform.position = new Vector3(_baseX, _baseY + _bobOffset, transform.position.z);
        }


        public void Init(Transform anchor, int index, float direction)
        {
            _spriteRenderer = _catchableNPC.SpriteRenderer;
            _anchor = anchor;
            _index = index;

            _baseX = transform.position.x; // 기존 위치의 X값을 기준으로 시작하여 부드럽게 붙게 함
            _baseY = _anchor.position.y; // Y는 앵커로 바로 붙음
            transform.position = new Vector3(_baseX, _baseY);

            // 풀링 때문에 내부 가속도 남아있을까봐 초기화
            _xVel = 0f;
            _yVel = 0f;

            SetDirection(direction);
            StartGhostAnimation();
        }

        public void OnDirectionChanged(float direction)
        {
            SetDirection(direction);
        }


        private void SetDirection(float direction)
        {
            _direction = direction < 0 ? -1f : 1f;
            _spriteRenderer.flipX = (direction > 0);
        }

        // NPC가 유령처럼 위 아래로 움직이는 애니메이션을 실행하는 메서드
        private void StartGhostAnimation()
        {
            _doTween?.Kill(); // tween이 있으면 초기화

            _bobOffset = 0f; // 시작점을 0으로 초기화

            // DoMoveY로 직접 y값 조절하지 않고 보간 값만 계산 후 Update에서 최종 대입
            _doTween = DOTween.To(
                () => _bobOffset,                           // getter: 현재 _bobOffset 값
                v => _bobOffset = v,                        // setter: 보간 값 할당
                _catchableNPC.Settings.BobAmplitude,        // 도착값: 0 ~ _bobAmplitude로 보간
                _catchableNPC.Settings.BobDuration          // 시간: 한 번 가는데 걸리는 시간
                )
                .SetEase(Ease.InOutSine)                            // ease 설정
                .SetLoops(-1, LoopType.Yoyo)                        // 무한 반복, 0 ~ _bobAmplitude
                .SetLink(
                gameObject,
                LinkBehaviour.KillOnDisable |                       // 비활성화될 때 Kill
                LinkBehaviour.KillOnDestroy                         // 파괴될 때 Kill
                );
        }
    }
}
