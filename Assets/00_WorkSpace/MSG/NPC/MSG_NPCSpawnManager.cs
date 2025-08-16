using Core.UnityUtil.PoolTool;

using MSG;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MSG_NPCSpawnManager : MonoBehaviour
{
    [SerializeField] private MSG_NPCSettings _settings;

    [SerializeField] private GameObject _handsomeNPC;
    [SerializeField] private GameObject _normalNPC;
    [SerializeField] private GameObject _uglyNPC;
    [SerializeField] private GameObject _rivalNPC;
    [SerializeField] private GameObject _disturbNPC;
    [SerializeField] private GameObject _bossNPC;

    private MSG_MapData _firstMap;
    private MSG_PlayerLogic _playerLogic;
    private float _lastTime;
    private bool[] _disturbSpawned;
    private bool _isBossSpawned = false;



    private void Awake()
    {
        _disturbSpawned = new bool[_settings.SpawnTimes.Length];
        for (int i = 0; i < _settings.SpawnTimes.Length; i++)
        {
            _disturbSpawned[i] = false;
        }
    }

    private void Start()
    {
        CreateNPCPool();
        InitSpawnSetting();
    }

    private void OnDisable()
    {
        if (YSJ_GameManager.Instance != null)
        {
            YSJ_GameManager.Instance.OnBatteryChanged -= CheckSpawnTiming;
        }
        if (_playerLogic != null)
        {
            _playerLogic.OnPlayerFeverStarted -= SpawnBossNPC;
        }
    }


    private void CreateNPCPool()
    {
        PoolManager.Instance.CreatePool("HandsomeNPC", _handsomeNPC);
        PoolManager.Instance.CreatePool("NormalNPC", _normalNPC);
        PoolManager.Instance.CreatePool("UglyNPC", _uglyNPC);
        PoolManager.Instance.CreatePool("RivalNPC", _rivalNPC);
        PoolManager.Instance.CreatePool("DisturbNPC", _disturbNPC);
        PoolManager.Instance.CreatePool("BossNPC", _bossNPC, 2);
    }

    private void InitSpawnSetting()
    {
        _playerLogic = MSG_PlayerReferenceProvider.Instance.GetPlayerLogic();
        _playerLogic.OnPlayerFeverStarted += SpawnBossNPC;
        _isBossSpawned = false;

        YSJ_GameManager.Instance.OnBatteryChanged += CheckSpawnTiming;

        _lastTime = YSJ_GameManager.Instance.BatteryPercent;

        _firstMap = _playerLogic.CurrentMap;
    }

    private void CheckSpawnTiming(float currentTime)
    {
        for (int i = 0; i < _disturbSpawned.Length; i++)
        {
            if (_disturbSpawned[i]) continue;

            float t = _settings.SpawnTimes[i];

            if (_lastTime > t && currentTime <= t)
            {
                SpawnDisturbNPC();
                _disturbSpawned[i] = true;
            }
        }

        _lastTime = currentTime;
    }

    // Disturb NPC 스폰: 60초, 80초에 한 번 플레이어가 있는 층에 플레이어 반대편에 스폰
    private void SpawnDisturbNPC()
    {
        MSG_MapData currentMap = _playerLogic.CurrentMap;

        Transform playerTransform = MSG_PlayerReferenceProvider.Instance.GetPlayerTransform();

        float xCenter = (currentMap.LeftPlayerEndPoint + currentMap.RightPlayerEndPoint) / 2;
        Vector2 spawnPos;

        if (playerTransform.position.x >= xCenter) // 플레이어가 오른쪽에 있으면
        {
            spawnPos = new Vector2(currentMap.LeftNPCEndPoint, currentMap.MiddleYPos);
        }
        else
        {
            spawnPos = new Vector2(currentMap.RightNPCEndPoint, currentMap.MiddleYPos);
        }
        PoolManager.Instance.Spawn("DisturbNPC", spawnPos, Quaternion.identity);
    }

    private void SpawnBossNPC()
    {
        if (_isBossSpawned) return; // 보스는 한 번만 스폰 가능

        // 1층 맵의 정 중앙에 스폰
        Vector2 spawnPos;
        int randomY = Random.Range(0, 2);

        if (randomY == 0)
        {
            spawnPos = new Vector2(_firstMap.XPos, _firstMap.TopYPos);
        }
        else
        {
            spawnPos = new Vector2(_firstMap.XPos, _firstMap.BottomYPos);
        }

        PoolManager.Instance.Spawn("BossNPC", spawnPos, Quaternion.identity);
        _isBossSpawned = true;
    }
}
