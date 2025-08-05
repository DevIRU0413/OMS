using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_MapEndPointBehaviour : MonoBehaviour
    {
        [SerializeField] private MSG_MapChanger _mapChanger;
        [SerializeField] private Direction _direction; // 어느 방향에 있는 끝 지점인지 설정
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private GameObject _arrowButton;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
            {
                YSJ_GameManager.Instance?.ReachedFloorEnd();

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

        /// <summary>
        /// 이동 버튼을 눌렀을 때 호출할 메서드.
        /// </summary>
        public void OnClickArrow()
        {
            _mapChanger.ChangeMap(_direction);
        }
    }
}
