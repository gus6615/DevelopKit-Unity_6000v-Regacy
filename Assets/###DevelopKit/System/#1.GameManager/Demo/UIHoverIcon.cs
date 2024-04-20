using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverIcon : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private UIHoverTest hover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hover = GameManager.Instance.UI.ShowHover<UIHoverTest>("UIHoverTest");
        hover.SetAnchoredPosition(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        hover.SetAnchoredPosition(eventData.position);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.UI.CloseHover();
        hover = null;
    }
}