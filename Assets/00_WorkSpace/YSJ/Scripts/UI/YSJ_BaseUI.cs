using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class YSJ_BaseUI : MonoBehaviour
{
    public abstract YSJ_UIType UIType { get; }

    private Dictionary<string, GameObject> uiObjectDic;
    private Dictionary<string, Component> componentDic;

    public Action OnOpenAction;
    public Action OnCloseAction;

    // private void Awake() => InitBaseUI();
    private void OnDestroy() => YSJ_UIManager.Instance.UnRegisterUI(this);

    public virtual void InitBaseUI()
    {
        InitRectObject();
        InitObjectCmp();

        YSJ_UIManager.Instance?.RegisterUI(this);
    }

    private void InitRectObject()
    {
        RectTransform[] recTrans = GetComponentsInChildren<RectTransform>(true);
        uiObjectDic = new Dictionary<string, GameObject>(recTrans.Length * 4);

        foreach (RectTransform child in recTrans)
        {
            uiObjectDic.TryAdd(child.gameObject.name, child.gameObject);
        }
    }
    private void InitObjectCmp()
    {
        Component[] components = GetComponentsInChildren<Component>(true);
        componentDic = new Dictionary<string, Component>(components.Length * 4);

        foreach (Component com in components)
        {
            componentDic.TryAdd($"{com.gameObject}_{com.GetType().Name}", com);
        }
    }

    public GameObject GetUI(in string objName)
    {
        uiObjectDic.TryGetValue(objName, out GameObject value);
        return value;
    }
    public T GetUI<T>(in string objName) where T : Component
    {
        componentDic.TryGetValue($"{objName}_{typeof(T).Name}", out Component com);

        if (com != null)
            return com as T;

        GameObject obj = GetUI(objName);

        if (obj == null)
            return null;

        com = obj.GetComponent<T>();

        if (com == null)
            return null;

        componentDic.TryAdd($"{objName}_{typeof(T).Name}", com);
        return com as T;

    }

    public YSJ_PointerHandler GetEvent(in string objName)
    {
        GameObject obj = GetUI(objName);
        if (obj == null)
        {
            Debug.LogWarning($"[YSJ_BaseUI] '{objName}' GameObject not found.");
            return null;
        }

        var handler = obj.GetComponent<YSJ_PointerHandler>();
        if (handler == null)
            handler = obj.AddComponent<YSJ_PointerHandler>();

        return handler;
    }

    public virtual void Open() { OnOpenAction?.Invoke(); }
    public virtual void Close() { OnCloseAction?.Invoke(); }
}
