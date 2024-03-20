using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    private Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    private UI_Scene _sceneUI = null;

    public GameObject UIRoot
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    public void SetCanvas(GameObject gameObject, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
            canvas.sortingOrder = _order++;
        else
            canvas.sortingOrder = 0;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject gameObject = Managers.Resource.Instantiate($"UI/Scene/{name}");

        T scene = Util.GetOrAddComponent<T>(gameObject);
        _sceneUI = scene;

        gameObject.transform.SetParent(UIRoot.transform);

        return scene;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject gameObject = Managers.Resource.Instantiate($"UI/Popup/{name}");

        T popup = Util.GetOrAddComponent<T>(gameObject);
        _popupStack.Push(popup);

        gameObject.transform.SetParent(UIRoot.transform);

        return popup;
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject gameObject = Managers.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            gameObject.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(gameObject);
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;

        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
