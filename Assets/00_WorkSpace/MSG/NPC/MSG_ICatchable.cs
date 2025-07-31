using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public interface MSG_ICatchable
    {
        public void OnCatchPressed();  // 마우스를 눌렀을 때
        public void OnCatchReleased(); // 마우스를 떼었을 때
        public void OnHoverEnter();    // 마우스가 대상 위에 올라갔을 때
        public void OnHoverExit();     // 마우스가 대상에서 벗어났을 때
    }
}
