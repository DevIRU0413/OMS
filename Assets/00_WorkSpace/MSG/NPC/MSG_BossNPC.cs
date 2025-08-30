using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_BossNPC : MSG_RivalNPC
    {
        private bool _isCaught = false;

        public bool IsCaught => _isCaught;

        public override void StartIdleAnim() => PlayIfPossible(MSG_AnimParams.BOSS_IDLE);
        public override void StartWalkAnim() => PlayIfPossible(MSG_AnimParams.BOSS_WALK);
        public override void StartCatchingAnim() => PlayIfPossible(MSG_AnimParams.BOSS_CATCHING);
        public override void StartSurprisedAnim() => PlayIfPossible(MSG_AnimParams.BOSS_SURPRISED);
        private void StartFollowingAnim() => PlayIfPossible(MSG_AnimParams.BOSS_FOLLOWING);


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void Update()
        {
            if (_isCaught) return;

            base.Update();
        }


        public override void StartCompeting(Transform target)
        {
            if (!IsOnScreen()) return; // 화면 밖에 있으면 return

            if (_isCompeteStarted) return; // 이미 경쟁 중이면 return

            _isCompeteStarted = true;
            _isCompeting = true; // 보스는 놀라는 애니메이션이 일단 없는 것 같아서 바로 시작

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

        public override void EndCompeting()
        {
            _isCompeteStarted = false;
            SetActiveFightObj(false);
            StartIdleAnim();
        }

        public void EnableInteraction()
        {
            _collider.enabled = true;
        }

        public override void LoseAndDespawn()
        {
            // 보스는 날아가지 않고 따라가기
            BecomeFollower();
        }


        protected override void OnEnableInitHooker() { base.OnEnableInitHooker(); }
        // 날아가는 애니메이션 코루틴은 안쓸 것이기 때문에 빈 본문 사용
        protected override void OnEnableAnimHooker() { }


        protected override IEnumerator CompeteAfterSurprising()
        {
            StartSurprisedAnim();
            yield return new WaitForSeconds(_surprisedAnimationClip.length);
            ForceStartAnim(MSG_AnimParams.BOSS_CATCHING);
            SetActiveFightObj(true);
            _isCompeting = true;
        }

        private void BecomeFollower()
        {
            EndCompeting();

            // 살아있을 때의 이동 중단
            _isCaught = true;
            _moveController.StopMovement();
            _collider.enabled = false;

            StartFollowingAnim();

            MSG_FollowManager.Instance.AddCapturedNPC(this);
        }
    }
}
