using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour
{
    [SerializeField] protected RectTransform target;
    protected Vector2 bound;
    protected Vector2 position;
    private bool isInit = false;

    public virtual void Init()
    {
        if (target == null)
        {
            Debug.LogError("Target is null");
            return;
        }

        bound = target.rect.size;
        isInit = true;
    }

    public void SetAnchoredPosition(Vector2 pos)
    {
        if (isInit == false)
            Init();

        Vector2 standardScreen = UIController.StandardScreen;
        float ratio = standardScreen.x / Screen.width;

        Vector2 anchorCenter = CalculateAnchorCenter();
        Vector2 pivot = target.pivot;
        Vector2 realPos;

        float minX = -standardScreen.x * 0.5f / ratio + pivot.x * bound.x - anchorCenter.x;
        float maxX = standardScreen.x * 0.5f / ratio - (1 - pivot.x) * bound.x - anchorCenter.x;
        float minY = -standardScreen.y * 0.5f / ratio + pivot.y * bound.y - anchorCenter.y;
        float maxY = standardScreen.y * 0.5f / ratio - (1 - pivot.y) * bound.y - anchorCenter.y;

        realPos.x = Mathf.Clamp(pos.x, minX, maxX);
        realPos.y = Mathf.Clamp(pos.y, minY, maxY);

        target.anchoredPosition = realPos;
        position = realPos;
    }

    // 앵커의 중심 위치를 계산
    private Vector2 CalculateAnchorCenter()
    {
        Vector2 anchorMin = target.anchorMin;
        Vector2 anchorMax = target.anchorMax;
        RectTransform parent = target.parent as RectTransform;

        if (parent == null)
        {
            Debug.LogError("RectTransform's parent is not existed.");
            return Vector2.zero;
        }

        Vector2 parentSize = parent.rect.size;
        Vector2 anchorCenterOffset = new Vector2(
            (anchorMin.x + anchorMax.x - 1) * parentSize.x / 2,
            (anchorMin.y + anchorMax.y - 1) * parentSize.y / 2
        );

        return anchorCenterOffset;
    }
}