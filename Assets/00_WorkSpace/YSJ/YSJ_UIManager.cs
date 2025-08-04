using Runtime.UI;
using Core.UnityUtil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YSJ_UIManager : SimpleSingleton<YSJ_UIManager>
{
    #region Fields

    private readonly Dictionary<YSJ_UIType, Canvas> _canvasMap = new();
    private YSJ_PopupController _popupController = new();

    #endregion

    #region Init

    protected override void Init()
    {
        base.Init();
        InitCanvasLayers();
        _popupController.Init();
    }

    private void InitCanvasLayers()
    {
        foreach (YSJ_UIType layer in System.Enum.GetValues(typeof(YSJ_UIType)))
        {
            CreateCanvasIfNotExists(layer);
        }
    }

    #endregion

    #region Canvas Management

    private void CreateCanvasIfNotExists(YSJ_UIType layer)
    {
        if (_canvasMap.ContainsKey(layer)) return;

        string name = $"{layer}_Canvas";
        Canvas canvas = CreateCanvas(new YSJ_CanvasProfile(name, RenderMode.ScreenSpaceOverlay, (int)layer));
        _canvasMap.Add(layer, canvas);
    }

    private Canvas CreateCanvas(YSJ_CanvasProfile profile)
    {
        GameObject go = new GameObject(profile.canvasName);
        go.transform.SetParent(this.transform); // UIManager �ڽ����� ���

        Canvas canvas = go.AddComponent<Canvas>();
        canvas.renderMode = profile.renderMode;
        canvas.sortingOrder = profile.sortingOrder;

        if (profile.renderMode == RenderMode.ScreenSpaceCamera)
            canvas.worldCamera = Camera.main;

        go.AddComponent<CanvasScaler>();
        go.AddComponent<GraphicRaycaster>();

        return canvas;
    }

    public Canvas GetCanvas(YSJ_UIType layer)
    {
        if (_canvasMap.TryGetValue(layer, out Canvas canvas))
            return canvas;

        CreateCanvasIfNotExists(layer);
        return _canvasMap[layer];
    }

    #endregion

    #region Clear Methods

    public void TypeClear(YSJ_UIType layer)
    {
        if (layer == YSJ_UIType.Popup)
        {
            var count = _popupController.GetPopupCount();
#if UNITY_EDITOR
            Debug.Log($"[UIManager] {layer} ���̾��� {count}�� UI ������Ʈ�� ���ŵǾ����ϴ�.");
#endif
            _popupController.CloseAll();
            return;
        }

        if (!_canvasMap.TryGetValue(layer, out Canvas canvas))
        {
#if UNITY_EDITOR
            Debug.LogWarning($"[UIManager] {layer} ���̾ �ش��ϴ� Canvas�� �����ϴ�.");
#endif
            return;
        }

        Transform parent = canvas.transform;
        int childCount = parent.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(parent.GetChild(i).gameObject);
        }

#if UNITY_EDITOR
        Debug.Log($"[UIManager] {layer} ���̾��� {childCount}�� UI ������Ʈ�� ���ŵǾ����ϴ�.");
#endif
    }

    public void AllClear()
    {
        foreach (var kvp in _canvasMap)
        {
            TypeClear(kvp.Key);
        }
#if UNITY_EDITOR
        Debug.Log("[UIManager] ��� Canvas ���̾��� UI ������Ʈ�� �����߽��ϴ�.");
#endif
    }

    #endregion

    #region Popup Methods

    public void ShowPopup(GameObject popup)
    {
        _popupController.Register(popup);
    }

    public void CloseTopPopup()
    {
        _popupController.CloseTop();
    }

    public void ClosePopup(GameObject popup)
    {
        _popupController.Unregister(popup);
    }

    #endregion
}
