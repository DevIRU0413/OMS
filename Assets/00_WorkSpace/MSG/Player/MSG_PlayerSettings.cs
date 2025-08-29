using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    // 각도 설정용
    [System.Serializable]
    public class LookDirectionAngleRange
    {
        public LookDirection direction;
        [Range(0f, 360f)] public float startAngle;
        [Range(0f, 360f)] public float endAngle;
    }

    [CreateAssetMenu(fileName = "MSG_PlayerSettings", menuName = "ScriptableObjects/MSG_PlayerSettings")]
    public class MSG_PlayerSettings : ScriptableObject
    {
        //[field: SerializeField] public float InvincibleTime { get; private set; }     // 무적시간 -> 애니메이션 클립 length로 대체
        [field: SerializeField] public float BlinkInterval { get; private set; }        // 깜빡임 간격, 무적시간(InvincibleTime)의 약수로 설정하는 것이 좋습니다
        [field: SerializeField] public float HPDecreasePerSecond { get; private set; }  // 경쟁이 아닌 포획 중 1초 당 HP 감소 값
        [field: SerializeField] public float HPDecreasePerClick { get; private set; }   // 경쟁일 때 포획 중 클릭 당 HP 감소 값, 주변 Rival NPC에 따라 배가 될 수 있음
        [field: SerializeField] public float FeverTimeDuration { get; private set; }    // Fever Time 지속시간
        [field: SerializeField] public float FeverScoreMagnifier { get; private set; }  // Fever 중 점수 배율
        [field: SerializeField] public float FeverGaugeIncreaseMagnifier { get; private set; } // Fever 중 게이지 증가 배율
        [field: SerializeField] public float DeathMoveSpeed { get; private set; }       // 사망 시 오른쪽으로 쭉 이동할 때의 이동 속도
        [field: SerializeField] public float KnockbackDistance { get; private set; }    // 밀릴 거리
        [field: SerializeField] public float KnockbackHeight { get; private set; }      // 포물선의 최고 높이

        [field: SerializeField] public List<LookDirectionAngleRange> DirectionAngleRanges; // 각 방향별 각도 설정. 0도가 위를 가리킵니다.
        [field: SerializeField] public Sprite[] _playerStopSprites; // 정지했을 때의 플레이어 스프라이트.
                                            // UpRight, Right, DownRight, DownLeft, Left, UpLeft 순으로 등록해야 합니다.
    }
}
