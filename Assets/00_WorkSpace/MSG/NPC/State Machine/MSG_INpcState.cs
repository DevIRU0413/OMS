using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    /*
    상태 목록
    
    Wandering:      배회 상태
    Aimed:          마우스가 올라간 상태
    Catching:       경쟁 중이지 않은, NPC를 잡는 상태
    Competing:      경쟁 중인 상태
    Captured:       잡힌 상태
    CaptureFailed:  잡기에 실패하여 사라진 상태

     */

    public interface MSG_INpcState
    {
        public void Enter();
        public void Update();
        public void Exit();


        public void OnHoverEnter();
        public void OnHoverExit();
        public void OnCatchPressed();
        public void OnCatchReleased();
    }
}
