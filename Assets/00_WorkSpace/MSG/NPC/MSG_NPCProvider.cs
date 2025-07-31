using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public static class MSG_NPCProvider
    {
        private static Dictionary<Collider2D, MSG_RivalNPC> _rivalNpcMap = new();
        private static Dictionary<Collider2D, MSG_CatchableNPC> _catchableNpcMap = new();


        public static void RegisterRival(MSG_RivalNPC rival, CapsuleCollider2D col)
        {
            _rivalNpcMap[col] = rival;
        }

        public static void UnregisterRival(MSG_RivalNPC rival, CapsuleCollider2D col)
        {
            _rivalNpcMap.Remove(col);
        }

        public static bool TryGetRival(CapsuleCollider2D col, out MSG_RivalNPC npc)
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

        public static void Clear()
        {
            _rivalNpcMap.Clear();
            _catchableNpcMap.Clear();
        }
    }
}
