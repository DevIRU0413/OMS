using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_DisturbNPC : MSG_NPCBase
    {
        private void Start()
        {
            MSG_NPCProvider.RegisterDisturb(this, _collider);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _animator.enabled = true;
        }

        private void Update()
        {
            _moveController.Tick();
        }

        private void OnDestroy()
        {
            MSG_NPCProvider.UnregisterDisturb(this, _collider);
        }


        #region Animation Methods

        public override void StartIdleAnim()
        {
            PlayIfPossible(MSG_AnimParams.DISTURB_IDLE);
        }

        public override void StartWalkAnim()
        {
            PlayIfPossible(MSG_AnimParams.DISTURB_WALK);
        }

        public override void StartCatchingAnim() { }
        public override void StartSurprisedAnim() { }

        #endregion
    }
}
