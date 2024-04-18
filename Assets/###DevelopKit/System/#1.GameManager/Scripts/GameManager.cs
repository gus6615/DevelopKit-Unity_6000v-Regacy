using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private DataController _data;
    public DataController Data => _data;

    [SerializeField] private ResourceController _resource;
    public ResourceController Resource => _resource;

    [SerializeField] private SaveController _save;
    public SaveController Save => _save;

    [SerializeField] private SceneController _scene;
    public SceneController Scene => _scene;

    [SerializeField] private SoundController _sound;
    public SoundController Sound => _sound;

    [SerializeField] private UIController _ui;
    public UIController UI => _ui;

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
