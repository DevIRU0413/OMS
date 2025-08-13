using System.Collections;
using System.Collections.Generic;
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
        public string Name;                 // 맵 이름
        public int CurrentFloor;            // 현재 맵 층 수
        public MapType MapType;             // 맵 타입
        public float XPos;                  // 맵의 X 중심 좌표
        public float YPos;                  // 맵의 Y 중심 좌표
        public Vector2 LeftSpawnPoint;      // 왼쪽 플레이어 스폰 포인트
        public Vector2 RightSpawnPoint;     // 오른쪽 플레이어 스폰 포인트

        public int HandsomeNPCSpawnCount;   // Handsome NPC 스폰 수
        public int NormalNPCSpawnCount;     // Normal NPC 스폰 수
        public int UglyNPCSpawnCount;       // Ugly NPC 스폰 수
        public int RivalNPCSpawnCount;      // 라이벌 NPC 스폰 수
        public int DisturbNPCSpawnCount;    // 방해 NPC 스폰 수
        public int BossNPCSpawnCount;       // 보스 NPC 스폰 수

        // 아래의 맵 데이터는 이동할 맵이 없다면 등록하지 않아야 됩니다
        public MSG_MapData LeftUpMap;       // 왼쪽 위와 이어질 맵
        public MSG_MapData LeftDownMap;     // 왼쪽 아래와 이어질 맵
        public MSG_MapData RightUpMap;      // 오른쪽 위와 이어질 맵
        public MSG_MapData RightDownMap;    // 오른쪽 아래와 이어질 맵

        public float LeftEndPoint;          // 맵의 왼쪽   경계점 (x축), 해당 지점 이상으로는 플레이어 및 NPC가 지나갈 수 없습니다
        public float RightEndPoint;         // 맵의 오른쪽 경계점 (x축), 해당 지점 이상으로는 플레이어 및 NPC가 지나갈 수 없습니다
    }
}
