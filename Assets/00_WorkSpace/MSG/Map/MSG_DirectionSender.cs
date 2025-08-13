using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_DirectionSender : MonoBehaviour
    {
        [SerializeField] private MSG_MapChanger _mapChanger;
        [SerializeField] private Direction _direction;

        public void OnClickMoveMap()
        {
            Debug.Log("OnClickMoveMap 호출");
            _mapChanger.ChangeMap(_direction);
        }
    }
}
