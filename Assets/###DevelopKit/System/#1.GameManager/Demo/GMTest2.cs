using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMTest2 : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            GameManager.Instance.Scene.ChangeScene(SceneType.MainScene);
    }
}
