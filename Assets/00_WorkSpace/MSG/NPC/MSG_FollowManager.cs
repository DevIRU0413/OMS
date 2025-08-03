using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_FollowManager : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        private List<MSG_CatchableNPC> _captured = new();

        public void AddCapturedNPC(MSG_CatchableNPC npc)
        {
            // CatchableNPC에서 MSG_FollowManager 오브젝트를 직렬화해서 갖고 있기
            // 포획 시 CatchableNPC에서 MSG_FollowManager.AddCapturedNPC에 자기 자신을 넘기기
            // MSG_CatchableNPC 오브젝트의 직렬화된 MSG_FollowController 인스턴스를 Public 프로퍼티로 열어두기
            // 그럼 MSG_CatchableNPC의 MSG_FollowController를 참조하여 Transform을 배정해서 넘겨주기
            // 이후 FollowController에서 Attatch 이후 게임이 끝날 때까지 따라다니기
        }
    }
}
