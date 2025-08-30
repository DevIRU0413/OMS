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
        public static readonly int PLAYER_WALK_LEFT = Animator.StringToHash("PlayerWalkLeft");
        public static readonly int PLAYER_WALK_RIGHT = Animator.StringToHash("PlayerWalkRight");
        public static readonly int PLAYER_RUN_LEFT = Animator.StringToHash("PlayerRunLeft");
        public static readonly int PLAYER_RUN_RIGHT = Animator.StringToHash("PlayerRunRight");

        //public static readonly int PLAYER_CATCHING = Animator.StringToHash("PlayerCatching");
        public static readonly int PLAYER_CATCHING_LEFT_UP = Animator.StringToHash("PlayerCatchingLeftUp");
        public static readonly int PLAYER_CATCHING_LEFT_DOWN = Animator.StringToHash("PlayerCatchingLeftDown");
        public static readonly int PLAYER_CATCHING_RIGHT_UP = Animator.StringToHash("PlayerCatchingRightUp");
        public static readonly int PLAYER_CATCHING_RIGHT_DOWN = Animator.StringToHash("PlayerCatchingRightDown");
        public static readonly int PLAYER_FEVER_LEFT = Animator.StringToHash("PlayerFeverLeft");
        public static readonly int PLAYER_FEVER_RIGHT = Animator.StringToHash("PlayerFeverRight");
        public static readonly int PLAYER_HIT = Animator.StringToHash("PlayerHit");

        // -- Catchable NPC
        public static readonly int CATCHABLE_IDLE = Animator.StringToHash("CatchableIdle");
        public static readonly int CATCHABLE_WALK = Animator.StringToHash("CatchableWalk");
        public static readonly int CATCHABLE_CATCHING = Animator.StringToHash("CatchableCatching");
        public static readonly int CATCHABLE_FOLLOWING = Animator.StringToHash("CatchableNPCFollowing");

        // -- Rival NPC
        public static readonly int RIVAL_IDLE = Animator.StringToHash("RivalIdle");
        public static readonly int RIVAL_WALK = Animator.StringToHash("RivalWalk");
        public static readonly int RIVAL_SURPRISED = Animator.StringToHash("RivalSurprised");
        public static readonly int RIVAL_CATCHING = Animator.StringToHash("RivalCatching");

        // -- Disturb NPC
        public static readonly int DISTURB_IDLE = Animator.StringToHash("DisturbIdle");
        public static readonly int DISTURB_WALK = Animator.StringToHash("DisturbWalk");

        // -- Boss NPC
        public static readonly int BOSS_WALK = Animator.StringToHash("BossWalk");
        public static readonly int BOSS_IDLE = Animator.StringToHash("BossIdle");
        public static readonly int BOSS_CATCHING = Animator.StringToHash("BossCatching");
        public static readonly int BOSS_SURPRISED = Animator.StringToHash("BossSurprised"); // 없을 수도?
        public static readonly int BOSS_FOLLOWING = Animator.StringToHash("BossFollowing");

        // -- Fight Effect
        public static readonly int FIGHT_EFFECT = Animator.StringToHash("FightEffect"); // 오브젝트가 단순히 활성화 및 상시 실행이라면 안쓸 수도 있음
    }
}
