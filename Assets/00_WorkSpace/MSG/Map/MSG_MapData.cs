using UnityEngine;


namespace MSG
{
    public enum Direction
    {
        LeftUp,
        LeftDown,
        RightUp,
        RightDown,
    }

    public enum MapType
    {
        // 일단 임시로 사용 함
        FirstFloor,
        SecondFloor,
        ThirdFloor,
    }

    [CreateAssetMenu(fileName = "MSG_MapData", menuName = "ScriptableObjects/MSG_MapData")]
    public class MSG_MapData : ScriptableObject
    {
        [Header("맵 설정")]
        public string Name;                       // 맵 이름
        public int CurrentFloor;                  // 현재 맵 층 수
        public MapType MapType;                   // 맵 타입
        public float XPos;                        // 맵의 X 중심 좌표
        public float YPos;                        // 맵의 Y 중심 좌표

        [Header("NPC 스폰 수 설정")]
        public int HandsomeNPCSpawnCount;         // Handsome NPC 스폰 수
        public int NormalNPCSpawnCount;           // Normal NPC 스폰 수
        public int UglyNPCSpawnCount;             // Ugly NPC 스폰 수
        public int RivalNPCSpawnCount;            // 라이벌 NPC 스폰 수
        public int DisturbNPCSpawnCount;          // 방해 NPC 스폰 수      * DisturbNPC의 스폰 방법이 변경되어 현재는 사용하지 않습니다. 0이 아닐 경우 일반 스폰 로직을 따릅니다*
        public int BossNPCSpawnCount;             // 보스 NPC 스폰 수

        // * 아래의 맵 데이터는 이동할 맵이 없다면 등록하지 않아야 됩니다 *
        [Header("맵 연결 설정")]
        public MSG_MapData LeftUpMap;             // 왼쪽 위와 이어질 맵
        public MSG_MapData LeftDownMap;           // 왼쪽 아래와 이어질 맵
        public MSG_MapData RightUpMap;            // 오른쪽 위와 이어질 맵
        public MSG_MapData RightDownMap;          // 오른쪽 아래와 이어질 맵

        [Header("NPC 및 플레이어 활동 반경 설정")]
        public float LeftPlayerEndPoint;          // 맵의 왼쪽   경계점 (x축), 해당 지점 이상으로는 플레이어가 지나갈 수 없습니다
        public float RightPlayerEndPoint;         // 맵의 오른쪽 경계점 (x축), 해당 지점 이상으로는 플레이어가 지나갈 수 없습니다
        public float LeftNPCEndPoint;             // 맵의 왼쪽   경계점 (x축), 해당 지점 이상으로는 NPC가 지나갈 수 없습니다. 또한 해당 반경 내에서 NPC가 스폰됩니다
        public float RightNPCEndPoint;            // 맵의 오른쪽 경계점 (x축), 해당 지점 이상으로는 NPC가 지나갈 수 없습니다. 또한 해당 반경 내에서 NPC가 스폰됩니다

        public Vector2 LeftPlayerSpawnPoint;      // 왼쪽 플레이어 스폰 포인트
        public Vector2 RightPlayerSpawnPoint;     // 오른쪽 플레이어 스폰 포인트

        public float TopYPos;                     // 플레이어, Disturb NPC를 제외한 NPC 다닐 맵의 맨 위 Y좌표
        public float MiddleYPos;                  // 플레이어, Disturb NPC가 다닐 맵의 중간 Y좌표
        public float BottomYPos;                  // 플레이어, Disturb NPC를 제외한 NPC 다닐 맵의 맨 아래 Y좌표
    }
}
