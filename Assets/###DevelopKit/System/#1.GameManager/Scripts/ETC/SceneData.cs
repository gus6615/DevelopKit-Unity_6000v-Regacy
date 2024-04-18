using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneSystem;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scriptable Object/SceneData")]
public class SceneData : ScriptableObject
{
    public SceneType Type;
    private Action loadCallback;
    private Action unloadCallback;

    public virtual void OnLoad()
    {
        if (loadCallback != null)
            loadCallback.Invoke();
    }

    public virtual void OnUnload()
    {
        if (unloadCallback != null)
            unloadCallback.Invoke();
    }

    public void AddLoadCallback(params Action[] callback)
    {
        foreach (var item in callback)
            loadCallback += item;
    }

    public void AddUnloadCallback(params Action[] callback)
    {
        foreach (var item in callback)
            unloadCallback += item;
    }

    public void ClearLoadCallback()
        => loadCallback = null;

    public void ClearUnloadCallback()
        => unloadCallback = null;
}