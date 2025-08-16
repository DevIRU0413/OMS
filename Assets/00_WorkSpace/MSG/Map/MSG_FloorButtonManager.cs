using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_FloorButtonManager : MonoBehaviour
    {
        [SerializeField] private MSG_DirectionSender[] _buttons;
        [SerializeField] private float _buttonDisableDuration = 0.2f;

        private float _elapsed = 0f;

        private bool CanClickButton => _elapsed >= _buttonDisableDuration;


        private void Update()
        {
            _elapsed += Time.deltaTime;
        }


        public bool TryClickButton()
        {
            if (!CanClickButton)
            {
                return false;
            }

            _elapsed = 0f;
            return true;
        }
    }
}
