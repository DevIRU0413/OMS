using System.Collections.Generic;

using Core.UnityUtil;

using Unity.VisualScripting;

using UnityEngine;

public class ManagerGroup : MonoBehaviour
{
    #region Singleton
    private static ManagerGroup _instance;
    public static ManagerGroup Instance
    {
        get
        {
            if (_instance == null)
            {
                string groupName = $"@{typeof(ManagerGroup).Name}";
                GameObject go = GameObject.Find(groupName);
                if (go == null)
                {
                    go = new GameObject(groupName);
                    DontDestroyOnLoad(go);
                }

                _instance = go.GetOrAddComponent<ManagerGroup>();
            }

            return _instance;
        }
    }
    #endregion

    #region PrivateVariables
    private List<IManager> _unregisteredManagers  = new(); // 미등록
    private List<IManager> _registeredManagers = new(); // 등록됨

    private bool _isManagersInitialized = false; // 초기화 중간 확인 및 매니저들 사용 여부 확인용
    #endregion

    #region PublicMethod

    public bool IsUseAble()
    {
        return _isManagersInitialized;
    }

    public void RegisterManager(IManager manager)
    {
        if (manager == null || _registeredManagers.Contains(manager) || _unregisteredManagers.Contains(manager))
            return;

        foreach (var m in _registeredManagers)
        {
            if (m.Equals(manager))
                return;
        }

        foreach (var m in _unregisteredManagers)
        {
            if (m.Equals(manager))
                return;
        }

        _unregisteredManagers.Add(manager);
    }

    public void RegisterManager(GameObject managerObject)
    {
        RegisterManager(managerObject?.GetComponent<IManager>());
    }

    public void RegisterManager(params IManager[] managers)
    {
        foreach (IManager m in managers) RegisterManager(m);
    }

    public void RegisterManager(params GameObject[] managerObjects)
    {
        foreach (GameObject go in managerObjects) RegisterManager(go);
    }

    public void InitializeManagers()
    {
        _isManagersInitialized = false;
        // SortManagersByPriorityAscending(_unregisteredManagers);

#if UNITY_EDITOR
        Debug.Log($"[ManagerGroup-Init]: Start Initialize");
#endif 
        foreach (var manager in _unregisteredManagers)
        {
            if (manager == null) continue;

            manager.Initialize();
            GameObject goM =  UnityUtilEx.SafeGetGameObject(manager);
            if (goM == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"[ManagerGroup-Init]: Have Not Manager GameObject");
#endif
                continue;
            }
#if UNITY_EDITOR
            Debug.Log($"[ManagerGroup-Init]: {goM.name}");
#endif
            _registeredManagers.Add(manager);
            goM.transform.SetParent(transform, false);
        }

        _unregisteredManagers.Clear();
        _isManagersInitialized = true;
    }

    /// <summary>
    /// 매니저들 내부 데이터 종료 처리 밎 정리
    /// </summary>
    public void CleanupManagers()
    {
#if UNITY_EDITOR
        Debug.Log($"[ManagerGroup-Cleanup]: Start Cleanup");
#endif 
        for (int i = 0; i < _registeredManagers.Count; i++)
        {
            IManager manager = _registeredManagers[i];

            if (manager == null)
            {
                _registeredManagers.Remove(manager);
                continue;
            }

            GameObject go = manager.GetGameObject();
            manager.Cleanup();
#if UNITY_EDITOR
            Debug.Log($"[ManagerGroup-Cleanup]: {go.name}");
#endif
        }
    }

    /// <summary>
    /// 지속적인 생존이 필요하지 않은 매니저 정리
    /// </summary>
    /// <param name="forceClear">강제 정리 여부</param>
    public void ClearManagers(bool forceClear = false)
    {
#if UNITY_EDITOR
        Debug.Log($"[ManagerGroup-Clear]: Start Clear");
#endif 
        for (int i = 0; i < _registeredManagers.Count; i++)
        {
            IManager manager = _registeredManagers[i];
            if (manager == null)                                        // 매니저 인터페이스가 없는 상태(오브젝트에 접근 불가)
            {
                _registeredManagers.RemoveAt(i);                        // 매니저가 없으니 Remove 사용 불가 > RemoveAt 사용
#if UNITY_EDITOR
                Debug.Log($"[ManagerGroup-Clear]: Remove Null Manager Interface(Rmove Index: {i})");
#endif 
                i--;                                                    // 매니저 제거로 인한 인덱스 에러 방지위한 -1
            }
            else if (!manager.IsDontDestroy || forceClear)
            {
                GameObject go = UnityUtilEx.SafeGetGameObject(manager); // 안전하게 게임오브젝트 확인
                manager.Cleanup();                                      // 매니저 클린업 진행
                _registeredManagers.Remove(manager);                    // 매니저 제거
                i--;                                                    // 매니저 제거로 인한 인덱스 에러 방지위한 -1
                string name = go.name;                                  // 제거 대상 이름 캐싱
                if (go) Destroy(go);                                    // 오브젝트 존재 시, 제거
#if UNITY_EDITOR
                Debug.Log($"[ManagerGroup-Clear]: {name}");
#endif 
            }
        }
    }

    public void ClearAllManagers()
    {
        ClearManagers(true);
    }

    public T GetManager<T>() where T : Component
    {
        foreach (var manager in _registeredManagers)
        {
            if (manager is T tManager)
            {
                return tManager as T;
            }
        }
        return null;
    }
    #endregion

    #region PrivateMethod

    /*
    private void SortManagersByPriorityAscending(List<IManager> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - i - 1; j++)
            {
                if (list[j].Priority > list[j + 1].Priority)
                {
                    IManager temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }
    }

    private void SortManagersByPriorityDescending(List<IManager> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            for (int j = 0; j < list.Count - i - 1; j++)
            {
                if (list[j].Priority < list[j + 1].Priority)
                {
                    IManager temp = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = temp;
                }
            }
        }
    }
    */

    #endregion
}
