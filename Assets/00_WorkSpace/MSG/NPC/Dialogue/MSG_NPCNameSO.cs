using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    [CreateAssetMenu(fileName = "MSG_NPCNameSO", menuName = "ScriptableObjects/MSG_NPCNameSO")]
    public class MSG_NPCNameSO : ScriptableObject
    {
        public List<string> NameList = new();
    }
}
