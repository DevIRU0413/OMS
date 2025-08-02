using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    [CreateAssetMenu(fileName = "MSG_PlayerSettings", menuName = "ScriptableObjects/MSG_PlayerSettings")]
    public class MSG_PlayerSettings : ScriptableObject
    {
        public float InvincibleTime; // 무적시간

    }
}
