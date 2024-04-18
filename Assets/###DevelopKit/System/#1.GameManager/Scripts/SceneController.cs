using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneSystem;
using System;

public enum SceneType
{
    MainScene,
    UITestScene,
}

public class SceneController : MonoBehaviour
{
    private const string fadeInAnimName = "SceneController_FadeIn";
    private const string fadeOutAnimName = "SceneController_FadeOut";
    private const float defaultFadeTime = 0.75f;

    [SerializeField] private Animation anim;
    [SerializeField] private Image blindImage;

    private List<SceneData> sceneDatas;
    private SceneData currentScene;

    private bool isLoading;
    public bool IsLoading => isLoading;

    public void Init()
    {
        sceneDatas = new List<SceneData>();
        sceneDatas.AddRange(GameManager.Instance.Resource.LoadAll<SceneData>("SO/SceneData"));

        // Scene 전환 이벤트 설정 가능
        SetLoadCallback(SceneType.MainScene, () => { Debug.Log("MainScene is loaded."); });
        SetUnloadCallback(SceneType.MainScene, () => { Debug.Log("MainScene is unloaded."); });
        SetLoadCallback(SceneType.UITestScene, () => { Debug.Log("UITestScene is loaded."); });
        SetUnloadCallback(SceneType.UITestScene, () => { Debug.Log("UITestScene is unloaded."); });

        currentScene = sceneDatas.Find(x => x.Type.ToString() == Scenes.GetActiveScene().name);
 
        blindImage.color = Color.clear;
        isLoading = false;
        FadeIn();
    }

    public void ChangeScene(SceneType scene, float fadeAnimTime = defaultFadeTime)
    {
        if (isLoading == true)
        {
            Debug.Log("Scene is loading, so work to change is cancled.");
            return;
        }

        StartCoroutine(SwitchScene(scene, fadeAnimTime));
    }

    IEnumerator SwitchScene(SceneType scene, float fadeTime)
    {
        isLoading = true;
        FadeOut(fadeTime);

        yield return new WaitForSeconds(fadeTime);

        currentScene.OnUnload();

        // 씬 전환
        Scenes.LoadScene((int)scene);

        currentScene = sceneDatas.Find(x => x.Type == scene);
        currentScene.OnLoad();

        FadeIn(fadeTime);

        yield return new WaitForSeconds(fadeTime);

        isLoading = false;
    }

    public void SetUnloadCallback(SceneType scene, Action unloadCallback)
    {
        SceneData sceneData = FindSceneData(scene);
        sceneData.ClearUnloadCallback();
        sceneData.AddUnloadCallback(unloadCallback);
    }

    public void SetLoadCallback(SceneType scene, Action loadCallback)
    {
        SceneData sceneData = FindSceneData(scene);
        sceneData.ClearLoadCallback();
        sceneData.AddLoadCallback(loadCallback);
    }

    private SceneData FindSceneData(SceneType scene)
    {
        SceneData sceneData = sceneDatas.Find(x => x.Type == scene);
        if (sceneData == null)
        {
            Debug.Log($"{scene} is null.");
            return null;
        }

        return sceneData;
    }

    private void FadeIn(float _fadeTime = defaultFadeTime)
    {
        anim.Play(fadeInAnimName);
        anim[fadeInAnimName].speed = 1f / _fadeTime;
        StartCoroutine(InvokeWithDelay(() => blindImage.raycastTarget = false, _fadeTime));
    }

    private void FadeOut(float _fadeTime = defaultFadeTime)
    {
        anim.Play(fadeOutAnimName);
        anim[fadeOutAnimName].speed = 1f / _fadeTime;
        StartCoroutine(InvokeWithDelay(() => blindImage.raycastTarget = true, _fadeTime));
    }

    IEnumerator InvokeWithDelay(Action action, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        action.Invoke();
    }
}
