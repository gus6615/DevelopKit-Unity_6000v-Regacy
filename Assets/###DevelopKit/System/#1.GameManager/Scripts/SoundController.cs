using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BGM,
    SFX
}

public class SoundController : MonoBehaviour
{
    private readonly string _audioMixerPath = "Sounds/AudioMixer";
    private AudioSource[] _audioSources;
    private Dictionary<string, AudioClip> _audioClipDic;
    private AudioMixer _audioMixer;

    public void Init()
    {
        string[] soundNames = System.Enum.GetNames(typeof(SoundType));
        _audioSources = new AudioSource[soundNames.Length];
        _audioClipDic = new Dictionary<string, AudioClip>();
        _audioMixer = GameManager.Resource.Load<AudioMixer>(_audioMixerPath);

        for (int i = 0; i < soundNames.Length; i++)
        {
            GameObject go = new GameObject(soundNames[i]);
            _audioSources[i] = go.AddComponent<AudioSource>();
            _audioSources[i].outputAudioMixerGroup = _audioMixer.FindMatchingGroups(soundNames[i])[0];
            go.transform.SetParent(this.transform);
        }
    }

    public void Play(AudioClip audioClip, SoundType soundType)
    {
        if (audioClip == null)
        {
            Debug.Log("Failed to Play : AudioClip is null");
            return;
        }

        AudioSource audioSource = _audioSources[(int)soundType];

        switch (soundType)
        {
            case SoundType.BGM:
                if (audioSource.isPlaying)
                    audioSource.Stop();

                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
                break;

            case SoundType.SFX:
                audioSource.PlayOneShot(audioClip);
                break;
        }
    }

    public void Play(string path, SoundType soundType)
    {
        AudioClip audioClip = GetOrAddAudioClip(path);
        Play(audioClip, soundType);
    }

    private float ConvertVolumetoDB(float volume)
        => Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20;

    public void SetMasterVolume(float volume)
        => _audioMixer.SetFloat("MasterVolume", ConvertVolumetoDB(volume));

    public void SetBGMVolume(float volume)
        => _audioMixer.SetFloat("BGMVolume", ConvertVolumetoDB(volume));

    public void SetSFXVolume(float volume)
        => _audioMixer.SetFloat("SFXVolume", ConvertVolumetoDB(volume));

    private AudioClip GetOrAddAudioClip(string path)
    {
        AudioClip audioClip = null;

        if (_audioClipDic.TryGetValue(path, out audioClip) == false)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
            if (audioClip == null)
            {
                Debug.Log($"Failed to GetOrAddAudioClip : {path}");
                return null;
            }

            _audioClipDic.Add(path, audioClip);
        }

        return audioClip;
    }
}
