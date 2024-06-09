using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    static public Vector2 StandardScreen = new Vector2(1920, 1080);

    [SerializeField] private Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    [SerializeField] private UI_Hover hover;

    private Transform root;
    public Transform Root
    {
        get
        {
            if (root == null)
            {
                GameObject go = GameObject.Find("@UI_Root");
                if (go == null)
                    go = new GameObject { name = "@UI_Root" };
                root = go.transform;
            }
            return root;
        }
    }

    private int sortingOrder;

    public UI_Popup CurrentPopup => popupStack.Count == 0 ? null : popupStack.Peek();
    public bool IsEmptyPopup => popupStack.Count == 0;

    public void Init()
    {
        ClearPopup();
        CloseHover();

        sortingOrder = 10;
    }

    public T ShowPopup<T>(string name) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/Popup/{name}", Root);
        SetCanvas(go);
        T popup = go.GetOrAddComponent<T>();
        popupStack.Push(popup);
        return popup;
    }

    public void ClosePopup()
    {
        if (IsEmptyPopup)
            return;

        Destroy(CurrentPopup.gameObject);
        popupStack.Pop();
        sortingOrder--;
    }

    public void ClearPopup()
    {
        while (IsEmptyPopup == false)
            ClosePopup();
    }

    public T ShowHover<T>(string name) where T : UI_Hover
    {
        if (hover != null)
            CloseHover();

        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = GameManager.Resource.Instantiate($"UI/Hover/{name}", Root);
        SetCanvas(go);
        T hoverLoaded = go.GetOrAddComponent<T>();
        hover = hoverLoaded;
        return hoverLoaded;
    }

    public void CloseHover()
    {
        if (hover == null)
            return;

        GameManager.Resource.Destroy(hover.gameObject);
        hover = null;
        sortingOrder--;
    }

    private void SetCanvas(GameObject go)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        CanvasScaler scaler = go.GetOrAddComponent<CanvasScaler>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = sortingOrder++;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    }
}
