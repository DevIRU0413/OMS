using Core.UnityUtil.PoolTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_MapChanger : MonoBehaviour
    {
        [SerializeField] private MSG_MapData _startMap;

        [Header("스폰포인트 랜덤 값 설정")]
        [SerializeField] private float _leftX;
        [SerializeField] private float _rightX;
        [SerializeField] private float _topY;
        [SerializeField] private float _middleY; // 방해 NPC용 Y좌표 (플레이어와 동일)
        [SerializeField] private float _bottomY;

        private HashSet<MapType> _visitedMaps = new();
        private MSG_MapData _currentMap;
        private Transform _playerTransform;

        private void Start()
        {
            _playerTransform = MSG_PlayerReferenceProvider.Instance.GetPlayerTransform();
            _currentMap = _startMap;

            Coroutine wait;
            wait = StartCoroutine(Wait()); // 생각한 구조는 시작 씬에서 풀링을 해서 문제가 없어야 되지만 현재는 씬이 하나라 대기 후 NPC 스폰함
        }

        public void ChangeMap(Direction direction)
        {
            MSG_MapData nextMap;

            // 왼쪽이면
            if (direction == Direction.Left)
            {
                nextMap = _currentMap.LeftMap;
            }
            // 오른쪽이면
            else
            {
                nextMap = _currentMap.RightMap;
            }

            if (nextMap != null)
            {
                _currentMap = nextMap;
            }

            ApplyMap(direction);
        }

        private void ApplyMap(Direction direction)
        {
            // 페이드 아웃 등의 효과

            // 어느 방향에서 플레이어가 스폰될 지
            // 근데 플레이어가 중앙에서 스폰되어야 되려나??
            if (direction == Direction.Left)
            {
                _playerTransform.position = _currentMap.LeftSpawnPoint;
            }
            else
            {
                _playerTransform.position = _currentMap.RightSpawnPoint;
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

            _visitedMaps.Add(_currentMap.MapType);
        }


        private void SpawnNPC()
        {
            PoolManager.Instance.Spawn("HandsomeNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
            PoolManager.Instance.Spawn("NormalNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
            PoolManager.Instance.Spawn("UglyNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
            PoolManager.Instance.Spawn("RivalNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
            PoolManager.Instance.Spawn("DisturbNPC", GetRandomSpawnPointForDisturb(), Quaternion.identity);
            PoolManager.Instance.Spawn("BossNPC", GetRandomSpawnPointForOther(), Quaternion.identity);
        }


        /// <summary>
        /// 방해 NPC의 스폰포인트를 랜덤으로 반환하는 메서드.
        /// 방해 NPC는 플레이어와 동일 선상에 위치함 (X 축이 같은, 즉 Y 좌표값이 같은 위치에서 활동함)
        /// </summary>
        private Vector2 GetRandomSpawnPointForDisturb()
        {
            float randomX = Random.Range(_leftX, _rightX);
            return new Vector2(randomX, _middleY + GetFloorHeight());
        }

        /// <summary>
        /// 방해 NPC가 아닌 다른 NPC의 스폰포인트를 랜덤으로 반환하는 메서드
        /// 다른 NPC는 플레이어와 동일하지 않은 Y 좌표값에 위치함
        /// </summary>
        /// <returns></returns>
        private Vector2 GetRandomSpawnPointForOther()
        {
            float randomX = Random.Range(_leftX, _rightX);

            float randomY;
            int randomYIndex = Random.Range(0, 2);

            if (randomYIndex == 0)
                randomY = _topY;
            else
                randomY = _bottomY;

            return new Vector2(randomX, randomY + GetFloorHeight());
        }

        // 이 방식이 괜찮은지 확인 필요
        // 현재는 한 층이 Y좌표 20만큼 차이나서 이 값을 스폰포인트에 더하는데, 연동되지 않고 하드코딩 느낌이 나서 좋은 것 같지는 않음
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
        }
    }
}
