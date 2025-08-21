using Core.UnityUtil.PoolTool;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace MSG
{
    public class MSG_MapChanger : MonoBehaviour
    {
        [SerializeField] private MSG_MapData _startMap;
        [SerializeField] private GameObject _cameraBound; // Cinemachine Confiner 2D를 쓸거라서 Bound 위치를 동적으로 옮기는 구조, 맵을 여러 개 추가해도 Bound를 하나만 두기 위함
        [SerializeField] private LayerMask _catchableLayer;

        private HashSet<MapType> _visitedMaps = new();
        private MSG_MapData _currentMap;
        private MSG_PlayerLogic _playerLogic;
        private Transform _playerTransform;
        private Collider2D[] _catchables = new Collider2D[20]; // 한 맵에 Catchable NPC 20명 초과하지 않을 것 같아서 설정. 많아지면 추후 변경

        private void Start()
        {
            _playerTransform = MSG_PlayerReferenceProvider.Instance.GetPlayerTransform();
            _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();

            _currentMap = _startMap;
            _cameraBound.transform.position = new Vector2(_currentMap.XPos, _currentMap.YPos);
            _playerLogic.ChangeCurrentMap(_currentMap);

            Coroutine wait;
            wait = StartCoroutine(Wait()); // 생각한 구조는 시작 씬에서 풀링을 해서 문제가 없어야 되지만 현재는 씬이 하나라 대기 후 NPC 스폰함
        }

        public void ChangeMap(Direction direction)
        {
            Debug.Log("ChangeMap 호출 받음");
            MSG_MapData nextMap;

            // 입력받은 방향에 따라 현재 맵 변경
            if (direction == Direction.LeftUp)
            {
                if (_currentMap.LeftUpMap == null) return; // 만약 이동할 수 없다면 return
                nextMap = _currentMap.LeftUpMap;
                YSJ_GameManager.Instance.MoveToUpFloor();
            }
            else if (direction == Direction.LeftDown)
            {
                if (_currentMap.LeftDownMap == null) return;
                nextMap = _currentMap.LeftDownMap;
                YSJ_GameManager.Instance.MoveToDownFloor();
            }
            else if (direction == Direction.RightUp)
            {
                if (_currentMap.RightUpMap == null) return;
                nextMap = _currentMap.RightUpMap;
                YSJ_GameManager.Instance.MoveToUpFloor();
            }
            else //(direction == Direction.RightDown) 일 때
            {
                if (_currentMap.RightDownMap == null) return;
                nextMap = _currentMap.RightDownMap;
                YSJ_GameManager.Instance.MoveToDownFloor();
            }

            _currentMap = nextMap;
            _cameraBound.transform.position = new Vector2(_currentMap.XPos, _currentMap.YPos);

            ApplyMap(direction);
            _playerLogic.ChangeCurrentMap(_currentMap);
        }

        private void ApplyMap(Direction direction)
        {
            // 페이드 아웃 등의 효과

            // 플레이어 스폰
            if (direction == Direction.LeftUp || direction == Direction.LeftDown)
            {
                _playerTransform.position = _currentMap.LeftPlayerSpawnPoint;
            }
            else
            {
                _playerTransform.position = _currentMap.RightPlayerSpawnPoint;
            }

            // 방문한 적이 없다면 스폰
            if (!_visitedMaps.Contains(_currentMap.MapType))
            {
                SpawnNPC();
            }
            else
            {
                Debug.Log("방문해서 스폰 안함");
            }

            // OverlapBox 검사 후 HiDialogue 호출
            StartCoroutine(CheckCatchNpcForHi());

            // 스폰 중복 검사용 HashSet에 Add
            _visitedMaps.Add(_currentMap.MapType);
        }


        private void SpawnNPC()
        {
            for (int handsome = 0; handsome < _currentMap.HandsomeNPCSpawnCount; handsome++) PoolManager.Instance.Spawn("HandsomeNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
            for (int normal = 0; normal < _currentMap.NormalNPCSpawnCount; normal++) PoolManager.Instance.Spawn("NormalNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
            for (int ugly = 0; ugly < _currentMap.UglyNPCSpawnCount; ugly++) PoolManager.Instance.Spawn("UglyNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
            for (int rival1 = 0; rival1 < _currentMap.RivalNPC1SpawnCount; rival1++) PoolManager.Instance.Spawn("RivalNPC1", GetRandomSpawnPointForOther(), Quaternion.identity);
            for (int rival2 = 0; rival2 < _currentMap.RivalNPC2SpawnCount; rival2++) PoolManager.Instance.Spawn("RivalNPC2", GetRandomSpawnPointForOther(), Quaternion.identity);
            for (int disturb = 0; disturb < _currentMap.DisturbNPCSpawnCount; disturb++) PoolManager.Instance.Spawn("DisturbNPC", GetRandomSpawnPointForDisturb(), Quaternion.identity);
            for (int boss = 0; boss < _currentMap.BossNPCSpawnCount; boss++) PoolManager.Instance.Spawn("BossNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
        }


        /// <summary>
        /// 방해 NPC의 스폰포인트를 랜덤으로 반환하는 메서드.
        /// 방해 NPC는 플레이어와 동일 선상에 위치함 (X 축이 같은, 즉 Y 좌표값이 같은 위치에서 활동함)
        /// </summary>
        private Vector2 GetRandomSpawnPointForDisturb()
        {
            float randomX = Random.Range(_currentMap.LeftNPCEndPoint, _currentMap.RightNPCEndPoint);
            //return new Vector2(randomX, _middleY + GetFloorHeight());
            return new Vector2(randomX, _currentMap.MiddleYPos + _currentMap.YPos); // GetFloorHeight 대신 YPos 그대로 사용
        }

        /// <summary>
        /// 방해 NPC가 아닌 다른 NPC의 스폰포인트를 랜덤으로 반환하는 메서드
        /// 다른 NPC는 플레이어와 동일하지 않은 Y 좌표값에 위치함
        /// </summary>
        /// <returns></returns>
        private Vector2 GetRandomSpawnPointForOther()
        {
            float randomX = Random.Range(_currentMap.LeftNPCEndPoint, _currentMap.RightNPCEndPoint);

            float randomY;
            int randomYIndex = Random.Range(0, 2);

            if (randomYIndex == 0)
                randomY = _currentMap.TopYPos;
            else
                randomY = _currentMap.BottomYPos;

            //return new Vector2(randomX, randomY + GetFloorHeight());
            return new Vector2(randomX, randomY); // GetFloorHeight 대신 YPos 그대로 사용
        }

        // 이 방식이 괜찮은지 확인 필요
        // 현재는 한 층이 Y좌표 20만큼 차이나서 이 값을 스폰포인트에 더하는데, 연동되지 않고 하드코딩 느낌이 나서 좋은 것 같지는 않음
        // 이거 쓰지 말고 차라리 MapData에 Y좌표 필드를 넣는게 좋을 듯
        private float GetFloorHeight()
        {
            float hieght = 0;
            switch (_currentMap.MapType)
            {
                case (MapType.FirstFloor):
                    hieght += 0;
                    break;
                case (MapType.SecondFloor):
                    hieght += 20;
                    break;
                case (MapType.ThirdFloor):
                    hieght += 40;
                    break;
            }

            return hieght;
        }


        // 생각한 구조는 시작 씬에서 풀링을 해서 문제가 없어야 되지만 현재는 씬이 하나라 대기 후 NPC 스폰함
        private IEnumerator Wait()
        {
            yield return new WaitUntil(() => PoolManager.Instance.HasPool("HandsomeNPC"));
            yield return new WaitUntil(() => PoolManager.Instance.HasPool("NormalNPC"));
            yield return new WaitUntil(() => PoolManager.Instance.HasPool("UglyNPC"));
            yield return new WaitUntil(() => PoolManager.Instance.HasPool("RivalNPC"));
            yield return new WaitUntil(() => PoolManager.Instance.HasPool("DisturbNPC"));
            yield return new WaitUntil(() => PoolManager.Instance.HasPool("BossNPC"));

            PoolManager.Instance.DebugPool();
            _visitedMaps.Add(_currentMap.MapType);
            SpawnNPC();

            StartCoroutine(CheckCatchNpcForHi());
        }

        // NPC가 활성화되고, OnEnable에서 Register하니까 같은 프레임에서 안되는 경우가 있어 1 프레임 대기
        private IEnumerator CheckCatchNpcForHi()
        {
            yield return null;

            Vector2 mapPos = new(_currentMap.XPos, _currentMap.YPos); // 맵의 중앙 좌표
            Vector2 mapSize = new Vector2((_currentMap.RightPlayerEndPoint - _currentMap.LeftPlayerEndPoint), 12f); // 맵의 크기
            int npcCount = Physics2D.OverlapBoxNonAlloc(mapPos, mapSize, 0f, _catchables, _catchableLayer);

            for (int i = 0; i < npcCount; i++)
            {
                if (MSG_NPCProvider.TryGetCatchable(_catchables[i], out MSG_CatchableNPC catchNPC))
                {
                    catchNPC.PrintHiDialogue();
                }
            }
        }
    }
}
