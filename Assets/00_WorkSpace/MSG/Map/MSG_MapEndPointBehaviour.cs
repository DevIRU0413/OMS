using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_MapEndPointBehaviour : MonoBehaviour
    {
        [SerializeField] private MSG_MapChanger _mapChanger;
        [SerializeField] private Direction _direction; // 어느 방향에 있는 끝 지점인지 설정, Up 및 Down은 상관없음, Left와 Right만 중요
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private LayerMask _npcLayer;
        [SerializeField] private GameObject _arrowButton;

        private MSG_PlayerLogic _playerLogic;


        private void Start()
        {
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
            {
                if (_playerLogic.IsFinished) return; // 게임 종료 이후 활성화 금지

                YSJ_GameManager.Instance?.ReachedFloorEnd();

                _arrowButton.SetActive(true);
            }

            if (((1 << collision.gameObject.layer) & _npcLayer) != 0)
            {
                if (MSG_NPCProvider.TryGetCatchable(collision, out var catchable))
                {
                    catchable.NPCMoveController.ReachEnd(_direction);
                }
                else if (MSG_NPCProvider.TryGetRival(collision, out var rival))
                {
                    rival.NPCMoveController.ReachEnd(_direction);
                }
                else if (MSG_NPCProvider.TryGetDisturb(collision, out var disturb))
                {
                    disturb.NPCMoveController.ReachEnd(_direction);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer & _playerLayer) != 0))
            {
                YSJ_GameManager.Instance?.ReachedFloorNotEnd();

                _arrowButton.SetActive(false);
            }
        }
    }
}
