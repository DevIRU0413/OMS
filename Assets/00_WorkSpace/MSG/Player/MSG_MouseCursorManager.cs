using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_MouseCursorManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _npcLayerMask;

        private MSG_PlayerLogic _playerLogic;
        private MSG_PlayerData _playerData;
        private SpriteRenderer _spriteRenderer;
        private MSG_ICatchable _currentHoverTarget;

        private void Start()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            _playerData = MSG_PlayerReferenceProvider.Instance.GetPlayerData();
            _spriteRenderer = _playerLogic.PlayerSpriteRenderer;
        }

        private void Update()
        {
            MoveByMousePos();
            CheckHoverTarget();
            HandleClick();
        }

        private void MoveByMousePos()
        {
            if (_currentHoverTarget != null) return; // 마우스가 포획 가능한 NPC 위에 있을 때는 이동하지 않음

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 playerPos = _playerLogic.transform.position;

            float deltaX = mouseWorldPos.x - playerPos.x;
            float absDelta = Mathf.Abs(deltaX);

            float speed = 0f;
            if (absDelta < 0.5f)
            {
                speed = 0f;
            }
            else if (absDelta < 2f)
            {
                speed = _playerData.WalkMoveSpeed;
            }
            else
            {
                speed = _playerData.RunSpeed;
            }

            float moveDir = Mathf.Sign(deltaX);
            FlipSprite(moveDir);

            Vector3 move = new Vector3(moveDir * speed, 0f, 0f);
            _playerLogic.transform.position += move * Time.deltaTime;
        }


        private void CheckHoverTarget()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            Collider2D hit = Physics2D.OverlapPoint(mousePos2D, _npcLayerMask);

            if (hit != null)
            {
                if (MSG_NPCProvider.TryGetCatchable(hit, out var catchable))
                {
                    if ((object)_currentHoverTarget != catchable)
                    {
                        _currentHoverTarget?.OnHoverExit();  // 이전 대상 해제
                        _currentHoverTarget = catchable;
                        _currentHoverTarget.OnHoverEnter();  // 새 대상 진입
                    }
                    return;
                }
            }

            // 아무것도 안 가리키고 있을 때
            if (_currentHoverTarget != null)
            {
                _currentHoverTarget.OnHoverExit();
                _currentHoverTarget = null;
            }
        }

        private void HandleClick()
        {
            if (_currentHoverTarget == null) return;

            // 마우스 버튼 다운
            if (Input.GetMouseButtonDown(0))
            {
                _currentHoverTarget.OnCatchPressed();
            }

            // 마우스 버튼 업
            if (Input.GetMouseButtonUp(0))
            {
                _currentHoverTarget.OnCatchReleased();
            }
        }

        // 플레이어가 이 로직 들고 있는게 자연스러울 듯
        private void FlipSprite(float moveDir)
        {
            if (_spriteRenderer == null) return;
            _spriteRenderer.flipX = moveDir < 0;
        }

        // TODO: Raycast로 ICollectible 인터페이스인 경우 MoveByMousePos() 멈추고 클릭 시 포획 시도
        // 만약 해당 NPC가 경쟁 중인 상태라면 클릭 연타로 변경
    }
}
