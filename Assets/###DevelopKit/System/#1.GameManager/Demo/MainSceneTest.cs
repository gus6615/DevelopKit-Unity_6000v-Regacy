using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneTest : MonoBehaviour
{
    private readonly Color[] slotColors = { Color.white, Color.gray };

    [SerializeField] private Slider masterSlider, bgmSlider, sfxSlider;

    private float masterVolume, bgmVolume, sfxVolume;

    // Start is called before the first frame update
    void Start()
    {
        masterVolume = bgmVolume = sfxVolume = 1f;
        masterSlider.value = bgmSlider.value = sfxSlider.value = 1f;
        masterSlider.onValueChanged.AddListener(OnMasterVolume);
        bgmSlider.onValueChanged.AddListener(OnBGMVolume);
        sfxSlider.onValueChanged.AddListener(OnSFXVolume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            GameManager.Scene.ChangeScene(SceneType.UITestScene);
    }

    public void OnBGMSlot(int bgmIndex)
        => GameManager.Sound.Play($"Sounds/BGM/Track_{bgmIndex}", SoundType.BGM);

    public void OnSFXSlot(int sfxIndex)
        => GameManager.Sound.Play($"Sounds/SFX/Track_{sfxIndex}", SoundType.SFX);

    public void OnMasterVolume(float value)
    {
        masterVolume = value;
        GameManager.Sound.SetMasterVolume(masterVolume);
    }

    public void OnBGMVolume(float value)
    {
        bgmVolume = value;
        GameManager.Sound.SetBGMVolume(bgmVolume);
    }

    public void OnSFXVolume(float value)
    {
        sfxVolume = value;
        GameManager.Sound.SetSFXVolume(sfxVolume);
    }
}
