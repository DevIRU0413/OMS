using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    /// <summary>
    /// 게임 종료 시 포획한 NPC의 FollowScore를 추가하기 위한 TriggerBox입니다.
    /// 체력이 떨어져 게임 종료된 경우가 아닌 시간이 끝나 종료될 때만 활성화됩니다.
    /// </summary>
    public class MSG_FollowScoreTriggerBox : MonoBehaviour
    {
        [SerializeField] private LayerMask _catchNpcLayer;
        private int _npcCount = 0;

        private void OnEnable()
        {
            foreach (var npc in MSG_FollowManager.Instance.CapturedList)
            {
                npc.EnableInteraction(); // 마우스 등에 상호작용 금지를 위해 꺼뒀던 콜라이더 활성화
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _catchNpcLayer.value) != 0)
            {
                if (!MSG_NPCProvider.TryGetCatchable(collision, out MSG_CatchableNPC catchNPC)) return; // Catchable NPC가 아니면 return
                if (!catchNPC.IsCaught) return; // 해당 Catchable NPC가 포획되지 않았으면 return

                int score = catchNPC.NPCData.FollowScore;
                YSJ_GameManager.Instance.AddScore(score);
                _npcCount++;

                if (MSG_FollowManager.Instance.CapturedList.Count == 0) // 포획한 NPC가 없으면
                {
                    YSJ_GameManager.Instance.GameResult(); // 즉시 점수 패널 호출
                }

                if (MSG_FollowManager.Instance.CapturedList.Count <= _npcCount) // 모든 NPC가 다 들어왔으면
                {
                    MSG_PlayerLogic playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
                    Debug.Log("[MSG_FollowScoreTriggerBox]: 팔로우 점수 계산 끝");

                    YSJ_GameManager.Instance.GameResult(); // 팔로워 다 지나가면 점수 패널 호출
                }

                // 머리 위에 뜨는 애니메이션에 대한 추가 로직이 필요하다면 여기서 score 변수를 보내 사용 및 추가
            }
        }
    }
}
