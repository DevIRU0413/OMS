using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_FollowController : MonoBehaviour
    {
        private Transform _targetTransform; // 플레이어 또는 앞의 NPC Transform
        private bool _isAttatched = false;  // 바로 붙는 것이 아닌 붙는 애니메이션이 있어야 자연스러울 것 같음


        private void Update()
        {
            if (!_isAttatched) return;

            Move();
        }


        private void Attatch()
        {
            // 붙기 시작하는 Lerp 등의 애니메이션

            _isAttatched = true;
        }

        private void Move()
        {
            
        }
    }
}
