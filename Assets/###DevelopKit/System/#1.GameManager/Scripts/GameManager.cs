using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DataController _data;
    public static DataController Data => Instance._data;

    [SerializeField] private ResourceController _resource;
    public static ResourceController Resource => Instance._resource;

    [SerializeField] private SaveController _save;
    public static SaveController Save => Instance._save;

    [SerializeField] private SceneController _scene;
    public static SceneController Scene => Instance._scene;

    [SerializeField] private SoundController _sound;
    public static SoundController Sound => Instance._sound;

    [SerializeField] private UIController _ui;
    public static UIController UI => Instance._ui;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _data.Init();
        _resource.Init();
        _save.Init();
        _scene.Init();
        _sound.Init();
        _ui.Init();
    }
}
