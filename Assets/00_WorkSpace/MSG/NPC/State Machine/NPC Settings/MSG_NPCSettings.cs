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
        public int MediumHeartDropStartRivalCount = 2;          // 중간 하트를 드랍하기 위해 필요한 최소 라이벌 수
        public int LargeHeartDropStartRivalCount = 3;           // 큰   하트를 드랍하기 위해 필요한 최소 라이벌 수

        //[Header("포획 완료 설정")]
        //public float CaptureDistance = 1.5f;                    // 포획 완료 시 NPC 간의 거리
    }
}
