using Runtime.UI;
using UnityEngine;

public class YSJ_UISpawnFactory
{
    public static GameObject SpawnUI(GameObject prefab, YSJ_UIType layer, int extraOrder = 0)
    {
        Canvas parentCanvas = YSJ_UIManager.Instance.GetCanvas(layer);
        if (parentCanvas == null)
        {
            Debug.LogError($"[UISpawnFactory] {layer}_Canvas�� UIManager�� �����Ǿ� ���� �ʽ��ϴ�.");
            return null;
        }

        GameObject ui = GameObject.Instantiate(prefab, parentCanvas.transform);

        // ���������� sorting order ����
        Canvas overrideCanvas = ui.GetComponent<Canvas>();
        int sortingOrder = (int)layer + extraOrder;
        if (overrideCanvas != null)
        {
            overrideCanvas.overrideSorting = true;
            overrideCanvas.sortingOrder = sortingOrder;
        }

        ui.transform.SetAsLastSibling();
        return ui;
    }

    public static GameObject SpawnUI(string resourcePath, YSJ_UIType layer, int extraOrder = 0)
    {
        GameObject prefab = Resources.Load<GameObject>(resourcePath);
        if (prefab == null)
        {
            Debug.LogError($"[UISpawnFactory] Prefab '{resourcePath}' not found!");
            return null;
        }

        return SpawnUI(prefab, layer, extraOrder);
    }

    public static GameObject ShowPopup(GameObject popupPrefab)
    {
        if (popupPrefab == null)
        {
            Debug.LogError("ShowPopup: popupPrefab�� �������");
            return null;
        }

        var popup = SpawnUI(popupPrefab, YSJ_UIType.Popup);

        // RectTransform ��ġ �ʱ�ȭ (Ǯ��ũ�� �߾�)
        var rect = popup.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        return popup;
    }
}
