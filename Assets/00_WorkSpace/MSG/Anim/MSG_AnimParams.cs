using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    /// <summary>
    /// 애니메이션 해싱용 static 클래스
    /// </summary>
    public static class MSG_AnimParams
    {
        // -- 플레이어
        public static readonly int PLAYER_IDLE = Animator.StringToHash("PlayerIdle"); // 해당 애니메이션은 LookByMouseDirection() 사용을 위해 키프레임이 없음
        public static readonly int PLAYER_WALK = Animator.StringToHash("PlayerWalk");
        public static readonly int PLAYER_RUN = Animator.StringToHash("PlayerRun");
        public static readonly int PLAYER_CATCHING = Animator.StringToHash("PlayerIdle");
        public static readonly int PLAYER_HIT = Animator.StringToHash("PlayerIdle");

        // -- Catchable NPC
        public static readonly int CATCHABLE_IDLE = Animator.StringToHash("CatchableIdle");
        public static readonly int CATCHABLE_WALK = Animator.StringToHash("CatchableWalk");
        public static readonly int CATCHABLE_CATCHING = Animator.StringToHash("CatchableCatching");
        //public static readonly int CATCHABLE_IDLE = Animator.StringToHash("CatchableIdle"); // 필요 없을 것 같아서 일단 주석

        // -- Rival NPC
        public static readonly int RIVAL_IDLE = Animator.StringToHash("RivalIdle");
        public static readonly int RIVAL_WALK = Animator.StringToHash("RivalWalk");
        public static readonly int RIVAL_SURPRISED = Animator.StringToHash("RivalSurprised");
        public static readonly int RIVAL_CATCHING = Animator.StringToHash("RivalCatching");

        // -- Disturb NPC
        public static readonly int DISTURB_IDLE = Animator.StringToHash("DisturbIdle");
        public static readonly int DISTURB_WALK = Animator.StringToHash("DisturbWalk");
    }
}
