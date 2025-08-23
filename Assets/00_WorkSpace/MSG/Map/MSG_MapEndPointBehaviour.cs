using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_MapEndPointBehaviour : MonoBehaviour
    {
        [SerializeField] private MSG_MapChanger _mapChanger;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private GameObject _arrowButton;
        [SerializeField] private Direction _direction;

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

                List<Direction> activables = DecideWhatButtonWillActive();
                // 여기서 activables 전달해야될 듯

                _arrowButton.SetActive(true);
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

        // 현재 위치에서 어떤 위치로 이동할 수 있는지 판단하여 가능한 방향을 Direction List로 반환하는 메서드
        // 리스트에는 LeftUp, LeftDown, RightUp, RightDown 이 포함될 수 있습니다.
        private List<Direction> DecideWhatButtonWillActive()
        {
            List<Direction> activableButtons = new();

            if (_direction == Direction.LeftUp || _direction == Direction.LeftDown)
            {
                if (_playerLogic.CurrentMap.LeftUpMap != null)
                {
                    activableButtons.Add(Direction.LeftUp);
                }
                if (_playerLogic.CurrentMap.LeftDownMap != null)
                {
                    activableButtons.Add(Direction.LeftDown);
                }
            }
            else
            {
                if (_playerLogic.CurrentMap.RightUpMap != null)
                {
                    activableButtons.Add(Direction.RightUp);
                }
                if (_playerLogic.CurrentMap.RightUpMap != null)
                {
                    activableButtons.Add(Direction.RightDown);
                }
            }

            return activableButtons;
        }
    }
}
