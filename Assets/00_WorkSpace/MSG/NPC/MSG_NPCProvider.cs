using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public static class MSG_NPCProvider
    {
        private static Dictionary<Collider2D, MSG_RivalNPC> _rivalNpcMap = new();
        private static Dictionary<Collider2D, MSG_CatchableNPC> _catchableNpcMap = new();
        private static Dictionary<Collider2D, MSG_DisturbNPC> _disturbNpcMap = new();


        public static void RegisterRival(MSG_RivalNPC rival, CapsuleCollider2D col)
        {
            _rivalNpcMap[col] = rival;
        }
        public static void UnregisterRival(MSG_RivalNPC rival, CapsuleCollider2D col)
        {
            _rivalNpcMap.Remove(col);
        }
        public static bool TryGetRival(Collider2D col, out MSG_RivalNPC npc)
        {
            return _rivalNpcMap.TryGetValue(col, out npc);
        }


        public static void RegisterCatchable(MSG_CatchableNPC catchable, Collider2D col)
        {
            _catchableNpcMap[col] = catchable;
        }
        public static void UnregisterCatchable(MSG_CatchableNPC catchable, Collider2D col)
        {
            _catchableNpcMap.Remove(col);
        }
        public static bool TryGetCatchable(Collider2D col, out MSG_CatchableNPC npc)
        {
            return _catchableNpcMap.TryGetValue(col, out npc);
        }

        public static void RegisterDisturb(MSG_DisturbNPC disturb, Collider2D col)
        {
            _disturbNpcMap[col] = disturb;
        }
        public static void UnregisterDisturb(MSG_DisturbNPC disturb, Collider2D col)
        {
            _disturbNpcMap.Remove(col);
        }
        public static bool TryGetDisturb(Collider2D col, out MSG_DisturbNPC npc)
        {
            return _disturbNpcMap.TryGetValue(col, out npc);
        }


        public static void Clear()
        {
            _rivalNpcMap.Clear();
            _catchableNpcMap.Clear();
            _disturbNpcMap.Clear();
        }
    }
}
