using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MSG
{
    [CreateAssetMenu(fileName = "MSG_DialogueSO", menuName = "ScriptableObjects/MSG_DialogueSO")]
    public class MSG_DialogueSO : ScriptableObject
    {
        public List<string> HiDialogue;
        public List<string> FollowDialogue;
        public List<string> LaughDialogue;
        public List<string> SuperChatDialogue; // 피버 상태일 때는 Follow 대신 슈퍼챗 대사 나와야 함
    }
}
