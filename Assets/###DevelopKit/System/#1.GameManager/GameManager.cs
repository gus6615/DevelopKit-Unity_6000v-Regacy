using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logger = NKStudio.Logger;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        Logger.Log("GameManager Start");
    }
}
