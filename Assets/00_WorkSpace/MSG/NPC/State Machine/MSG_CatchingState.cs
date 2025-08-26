using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_CatchingState : MSG_INpcState
    {
        private MSG_PlayerLogic _playerLogic;
        private MSG_CatchableNPC _npc;
        private Coroutine _waitForCheckRivalCO;

        public MSG_CatchingState(MSG_CatchableNPC npc)
        {
            _npc = npc;
        }


        public void Enter()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();

            // 만약 X가 왼쪽
            // 만약 Y가 위쪽
            // 아니면 아래쪽

            // 아니면 X가 오른쪽
            // 만약 Y가 위쪽
            // 아니면 아래쪽

            if (_npc.transform.position.x - _playerLogic.transform.position.x > 0) // npc가 플레이어 오른쪽에 있을 때
            {
                if (_npc.transform.position.y - _playerLogic.transform.position.y > 0) // npc가 플레이어 위에 있을 때
                {
                    // 즉, 1사분면
                    _playerLogic.Animator.Play(MSG_AnimParams.PLAYER_CATCHING_RIGHT_UP);
                }
                else // 아래에 있을 때
                {
                    // 즉, 4사분면
                    _playerLogic.Animator.Play(MSG_AnimParams.PLAYER_CATCHING_RIGHT_DOWN);
                }
            }
            else // npc가 플레이어 왼쪽에 있을 때
            {
                if (_npc.transform.position.y - _playerLogic.transform.position.y > 0) // npc가 플레이어 위에 있을 때
                {
                    // 즉, 2사분면
                    _playerLogic.Animator.Play(MSG_AnimParams.PLAYER_CATCHING_LEFT_UP);
                }
                else // 아래에 있을 때
                {
                    // 즉, 3사분면
                    _playerLogic.Animator.Play(MSG_AnimParams.PLAYER_CATCHING_LEFT_DOWN);
                }
            }

            // 그럼 회전은 필요 없음
            // 포획 중 플레이어 스프라이트 회전
            //if (_playerLogic.transform.position.x > _npc.transform.position.x)
            //{
            //    _playerLogic.PlayerSpriteRenderer.flipX = true;
            //}
            //else
            //{
            //    _playerLogic.PlayerSpriteRenderer.flipX = false;
            //}

            _npc.StartCaptureGauge();
            _npc.PrintLaughDialogue();
            _npc.StartCatchingAnim();

            if (_waitForCheckRivalCO != null)
            {
                _npc.StopCoroutine(_waitForCheckRivalCO);
            }
            _waitForCheckRivalCO = _npc.StartCoroutine(WaitAndCheckRival());

            YSJ_GameManager.Instance.OnChangedOver += StopAll;
        }

        public void Update()
        {
            if (_playerLogic.IsFallen) return; // 피격 시 게이지 증가 중지

            if (_playerLogic.IsFever)
            {
                _npc.IncreaseGauge(_npc.Settings.CaptureGaugeIncreasePerSecond *
                    _playerLogic.PlayerSettings.FeverGaugeIncreaseMagnifier * Time.deltaTime); // 피버 시 포획 중 초당 포획 게이지 증가
            }
            else
            {
                _npc.IncreaseGauge(_npc.Settings.CaptureGaugeIncreasePerSecond * Time.deltaTime); // 포획 중 초당 포획 게이지 증가
            }

            _playerLogic.TakeDamage(_playerLogic.PlayerSettings.HPDecreasePerSecond * Time.deltaTime);  // 포획 중 초당 체력 감소

            if (_npc.IsGaugeFull())
            {
                _npc.ChangeState(new MSG_CapturedState(_npc));
            }
        }

        public void Exit()
        {
            _npc.StopCaptureGauge();

            if (YSJ_GameManager.Instance != null)
            {
                YSJ_GameManager.Instance.OnChangedOver -= StopAll;
            }
        }

        public void OnCatchReleased()
        {
            _npc.ChangeState(new MSG_AimedState(_npc));
        }

        private IEnumerator WaitAndCheckRival()
        {
            yield return new WaitForSeconds(_npc.Settings.StartDetectionDelay); // 시작 지연 시간 후에 경쟁자 확인

            if (!_npc.IsPressed)
            {
                Debug.Log("눌려지지 않아서 경쟁상태로 전환 취소");
                yield break; // 지금도 눌리고 있는지 확인
            }

            if (_npc.HasNearbyRival())
            {
                _npc.ChangeState(new MSG_CompetingState(_npc)); // 경쟁 상태로 전환
            }
        }

        private void StopAll()
        {
            _npc.ChangeState(new MSG_WanderingState(_npc));
            _npc.ForceStartAnim(MSG_AnimParams.CATCHABLE_IDLE);
        }

        #region Unused Methods
        public void OnCatchPressed() { }
        public void OnHoverEnter() { }
        public void OnHoverExit() { }
        #endregion
    }

}
