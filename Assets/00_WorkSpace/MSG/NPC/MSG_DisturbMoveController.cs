using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_DisturbMoveController : MSG_NPCMoveController
    {
        [SerializeField] private MSG_DisturbNPC _me; // 자기 자신의 DisturbNPC
        // 강제 이동 관련
        private MSG_PlayerLogic _playerLogic;
        private bool _isForcedMove;
        private float _forcedMoveEndTime;
        private Vector2 _forcedDir;


        private void Start()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            if (_playerLogic != null)
            {
                _playerLogic.OnPlayerDamaged += MoveWhenTriggered;
            }

            _npcBase.StartWalkAnim();
            SetSpawnSettings();
        }

        private void OnDisable()
        {
            if (_playerLogic != null)
            {
                _playerLogic.OnPlayerDamaged -= MoveWhenTriggered;
            }
        }


        public override void Tick()
        {
            // 피격 시 강제 이동 우선 처리
            if (_isForcedMove)
            {
                float speed = _dataSO.CharWalkSpeed * _settings.ForcedMoveSpeedMultiplier;
                transform.Translate(_forcedDir * Time.deltaTime * speed);

                if (Time.time >= _forcedMoveEndTime)
                {
                    _isForcedMove = false;
                }
                return;
            }

            transform.Translate(_moveDirection * _dataSO.CharWalkSpeed * Time.deltaTime);
        }

        public override void ReachEnd(Direction endReachedDirection)
        {
            Debug.Log($"{gameObject.name}이 맵 경계에 부딪힘");

            // 경계 닿으면 반대로 이동
            _moveDirection = (_moveDirection == Vector2.left) ? Vector2.right : Vector2.left;
            _spriteRenderer.flipX = (_moveDirection == Vector2.left);

            // 강제 이동 중이면 종료하고 직진 유지
            if (_isForcedMove)
            {
                _isForcedMove = false;
            }

            _isMoving = true;
        }


        protected override void Init()
        {
            base.Init();
        }


        // 플레이어와 피격 시 무한 피격을 막기 위해 강제 이동
        // 요구 사항은 바라보는 방향으로 쭉 가는 것
        private void MoveWhenTriggered(MSG_DisturbNPC disturb)
        {
            if (_me == disturb) // 플레이어를 공격한 NPC가 나일 때만 빨라지게
            {
                _forcedDir = _spriteRenderer.flipX ? Vector2.left : Vector2.right; // 현재 NPC가 바라보는 방향 설정

                _isForcedMove = true;
                _forcedMoveEndTime = Time.time + _settings.ForcedMoveDuration;
            }
        }

        private void SetSpawnSettings()
        {
            if (_playerLogic == null)
            {
                _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            }

            _isForcedMove = false;

            if ((transform.position.x - _playerLogic.CurrentMap.XPos) > 0) // 맵 중앙 기준 오른쪽에서 스폰되었다면
            {
                _moveDirection = Vector2.left; // 왼쪽으로 이동 시작
            }
            else
            {
                _moveDirection = Vector2.right;
            }

            _isMoving = true;
            _spriteRenderer.flipX = (_moveDirection == Vector2.left);
        }
    }
}
