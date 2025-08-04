using Core.UnityUtil.PoolTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSG_NPCSpawnManager : MonoBehaviour
{
    // 각 NPC의 외형이 다른가??
    [SerializeField] private GameObject _handsomeNPC;
    [SerializeField] private GameObject _normalNPC;
    [SerializeField] private GameObject _uglyNPC;
    [SerializeField] private GameObject _rivalNPC;
    [SerializeField] private GameObject _disturbNPC;
    [SerializeField] private GameObject _bossNPC;

    private void Start()
    {
        PoolManager.Instance.CreatePool("HandsomeNPC", _handsomeNPC);
        PoolManager.Instance.CreatePool("NormalNPC", _normalNPC);
        PoolManager.Instance.CreatePool("UglyNPC", _uglyNPC);
        PoolManager.Instance.CreatePool("RivalNPC", _rivalNPC);
        PoolManager.Instance.CreatePool("DisturbNPC", _disturbNPC);
        PoolManager.Instance.CreatePool("BossNPC", _bossNPC, 2);
    }
}
