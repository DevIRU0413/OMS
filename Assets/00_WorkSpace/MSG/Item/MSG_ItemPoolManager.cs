using Core.UnityUtil;
using Core.UnityUtil.PoolTool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MSG
{
    public class MSG_ItemPoolManager : MonoBehaviour
    {
        [SerializeField] private GameObject _smallHeartPrefab;
        [SerializeField] private GameObject _mediumHeartPrefab;
        [SerializeField] private GameObject _largeHeartPrefab;
        [SerializeField] private GameObject _xLargeHeartPrefab;
        [SerializeField] private GameObject _xxLargeHeartPrefab;
        [SerializeField] private GameObject _breadBagPrefab;

        private void Start()
        {
            PoolManager.Instance.CreatePool("SmallHeartPool", _smallHeartPrefab, 5);
            PoolManager.Instance.CreatePool("MediumHeartPool", _mediumHeartPrefab, 5);
            PoolManager.Instance.CreatePool("LargeHeartPool", _largeHeartPrefab, 5);
            PoolManager.Instance.CreatePool("XLargeHeartPool", _largeHeartPrefab, 5);
            PoolManager.Instance.CreatePool("XXLargeHeartPool", _largeHeartPrefab, 5);
            PoolManager.Instance.CreatePool("BreadBagPool", _breadBagPrefab, 2);
        }
    }
}
