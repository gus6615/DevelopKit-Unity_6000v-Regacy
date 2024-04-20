using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopupTest : UI_Popup
{
    [SerializeField] private TMP_Text description;

    public readonly Vector2 MoveVec = new Vector2(50, -50);

    public void Init(int id, Vector2 startPos)
    {
        SetDescription($"This is UI Popup for example!\n'<color=#FF9696>ID: {id}</color>' UI Popup!");
        SetAnchoredPosition(startPos);
    }

    private void SetDescription(string info)
        => description.SetText(info);

    public void OnClosePopup()
    {
        GameManager.Instance.UI.ClosePopup();
        GMTest2.currentPopupID--;
        GMTest2.currentPopupPos -= MoveVec;
    }
}
