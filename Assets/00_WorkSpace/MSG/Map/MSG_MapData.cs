using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public enum Direction
    {
        Left,
        Right
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
        public MapType MapType;             // 맵 타입
        public Vector2 LeftSpawnPoint;      // 왼쪽 플레이어 스폰 포인트
        public Vector2 RightSpawnPoint;     // 오른쪽 플레이어 스폰 포인트

        public int HandsomeNPCSpawnCount;   // Handsome NPC 스폰 수
        public int NormalNPCSpawnCount;     // Normal NPC 스폰 수
        public int UglyNPCSpawnCount;       // Ugly NPC 스폰 수
        public int RivalNPCSpawnCount;      // 라이벌 NPC 스폰 수
        public int DisturbNPCSpawnCount;    // 방해 NPC 스폰 수
        public int BossNPCSpawnCount;       // 보스 NPC 스폰 수

        public MSG_MapData LeftMap;         // 왼쪽과 이어질 맵
        public MSG_MapData RightMap;        // 오른쪽과 이어질 맵
    }
}
