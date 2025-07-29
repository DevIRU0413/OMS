using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;


namespace MSG
{
    public class MSG_MouseCursorManager : MonoBehaviour
    {
        private MSG_PlayerLogic _playerLogic;
        private MSG_PlayerData _playerData;

        private void Start()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
            _playerData = MSG_PlayerReferenceProvider.Instance.GetPlayerData();
        }

        private void Update()
        {
            MoveByMousePos();
        }

        private void MoveByMousePos()
        {
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
            Vector3 move = new Vector3(moveDir * speed, 0f, 0f);
            _playerLogic.transform.position += move * Time.deltaTime;
        }

        // TODO: Raycast로 ICollectible 인터페이스인 경우 MoveByMousePos() 멈추고 클릭 시 포획 시도
        // 만약 해당 NPC가 경쟁 중인 상태라면 클릭 연타로 변경
    }
}
