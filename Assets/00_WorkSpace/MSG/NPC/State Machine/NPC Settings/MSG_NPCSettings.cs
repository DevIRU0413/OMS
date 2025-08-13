using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    /// <summary>
    /// NPC의 설정값을 담고 있는 ScriptableObject입니다.
    /// 데이터 테이블에 담기 애매한, 실시간으로 조절해야할 수치를 담고 있습니다.
    /// </summary>
    [CreateAssetMenu(fileName = "MSG_NPCSettings", menuName = "ScriptableObjects/MSG_NPCSettings")]
    public class MSG_NPCSettings : ScriptableObject
    {
        [Header("NPC 감지 설정")]
        public Vector2 DetectionSize = new Vector2(2f, 2f);     // 주변 라이벌 NPC를 감지하는 크기 
        public float StartDetectionDelay = 0.5f;                // NPC가 라이벌 NPC를 찾기 시작하기 전까지 걸리는 시간

        [Header("배회 상태 설정")]
        public float MinDuration = 1f;                          // 최소 행동 시간, 최소 해당 시간동안은 멈춰있거나 움직임
        public float MaxDuration = 3f;                          // 최대 행동 시간, 최대 해당 시간동안 멈춰있거나 움직임
        public float MoveProbability = 0.5f;                    // 이동할 확률, 1이면 항상 이동, 0이면 절대 이동하지 않음

        [Header("포획 관련 설정")]
        public float CaptureGaugeIncreasePerSecond = 50f;       // 초당 포획 게이지 증가 양
        public float CaptureGaugeIncreasePerClick = 10f;        // 클릭당 포획 게이지 증가 양

        [Header("하트 드랍 관련 설정")]
        [Min(0)] public int SmallHeartDropStartRivalCount = 0;  // 작은 하트를 드랍하기 위해 필요한 최소 라이벌 수
        public int MediumHeartDropStartRivalCount = 1;          // 중간 하트를 드랍하기 위해 필요한 최소 라이벌 수
        public int LargeHeartDropStartRivalCount = 2;           // 큰   하트를 드랍하기 위해 필요한 최소 라이벌 수
        public int XLargeHeartDropStartRivalCount = 3;          // XL   하트를 드랍하기 위해 필요한 최소 라이벌 수
        public int XXLargeHeartDropStartRivalCount = 4;         // XXL  하트를 드랍하기 위해 필요한 최소 라이벌 수

        [Header("맵 이탈 방지 관련 설정")]
        public int ForceMoveCount = 3;                          // 맵 끝에 다달았을 때 반대로 무조건 이동하게 하는 횟수

        [Header("피격 시 강제 이동 설정")]
        public float ForcedMoveDuration = 2f;                   // 방해 NPC가 플레이어와 부딪혔을 때, 강제로 이동하게 할 시간 (초)
        public float ForcedMoveSpeedMultiplier = 1f;            // 방해 NPC가 플레이어와 부딪혔을 때, 강제로 이동할 때의 속도 배율

        [Header("경쟁 승리 시 라이벌 애니메이션 설정")]
        public float MinAngle = -45f;                           // 0도가 위일 때를 기준으로 최소 각도
        public float MaxAngle = 45f;                            // 0도가 위일 때를 기준으로 최대 각도
        public float ImpulsePower = 10f;                        // NPC가 날아가는 힘, 높을 수록 위로 높이 올라갑니다
        public float GravityScale = 3f;                         // NPC가 날아갈 때의 중력 가속도, 높을 수록 빠르게 떨어집니다
        public float DespawnTime = 3f;                          // NPC가 날아가기 시작할 때부터 없어질 때까지의 시간

        [Header("포획 후 NPC 움직임 애니메이션 설정")]
        public float NpcDistance = 2f;                          // 포획된 NPC 간의 이격 거리
        public float SmoothSpeed = 5f;                          // 부드럽게 따라오는 속도
        public float AttachSpeed = 2f;                          // 포획 직후 플레이어에게 붙는 속도, 값이 작을수록 빠르게 붙습니다


        //[Header("포획 완료 설정")]
        //public float CaptureDistance = 1.5f;                    // 포획 완료 시 NPC 간의 거리
    }
}
