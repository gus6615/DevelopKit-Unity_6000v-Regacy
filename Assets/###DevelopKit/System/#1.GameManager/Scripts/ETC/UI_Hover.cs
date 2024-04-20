using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Hover : UI_Base
{
    public override void Init()
    {
        base.Init();

        // Hover의 Pivot은 좌하단으로 설정
        target.pivot = Vector2.zero;
    }
}