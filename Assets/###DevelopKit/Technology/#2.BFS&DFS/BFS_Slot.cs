using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotType
{
    Empty,
    Start,
    End,
    Wall
}

public class BFS_Slot : MonoBehaviour
{
    private readonly Color wallColor = Color.black;
    private readonly Color unVisitedColor = Color.white;
    private readonly Color visitedColor = Color.gray;

    private readonly Color startColor = Color.blue;
    private readonly Color endColor = Color.green;

    [SerializeField] private Image slotImage;

    private SlotType slotType;
    public SlotType SlotType => slotType;

    private bool isVisited;
    public bool IsVisited => isVisited;

    public void Init(SlotType slotType)
    {
        switch (slotType)
        {
            case SlotType.Empty: slotImage.color = unVisitedColor; break;
            case SlotType.Start: slotImage.color = startColor; break;
            case SlotType.End: slotImage.color = endColor; break;
            case SlotType.Wall: slotImage.color = wallColor; break;
        }

        this.slotType = slotType;
        isVisited = false;
    }

    public bool Visit()
    {
        isVisited = true;
        slotImage.color = visitedColor;
        return slotType == SlotType.End;
    }
}
