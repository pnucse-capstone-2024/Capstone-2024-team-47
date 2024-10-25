using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // NetworkManager
    NetworkManager _networkManager = new NetworkManager();
    public static NetworkManager NetworkManager { get { return Instance._networkManager; } }

    // SceneManager
    SceneDataManager _sceneManager = new SceneDataManager();
    public static SceneDataManager SceneManager { get { return Instance._sceneManager; } }

    // GameManager
    GameManager _gameManager = new GameManager();
    public static GameManager GameManager { get { return Instance._gameManager; } }

    // MapManager
    MapManager _mapManager = new MapManager();
    public static MapManager MapManager { get { return Instance._mapManager; } }

    // Manager
    static Manager _instance = null;
    public static Manager Instance { get { return _instance; } }

    void Start()
    {
        Init();
    }

    void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Manager>();
                go.AddComponent<PopupManager>();
            }

            Screen.SetResolution(1280, 720, false);

            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Manager>();
            _instance._networkManager.Init();
            _instance._sceneManager.Init();
            _instance._gameManager.Init();
            _instance._mapManager.Init();
        }
    }

    void Update()
    {
        _networkManager.Update();
        _sceneManager.Update();
        _gameManager.Update();
        _mapManager.Update();
        LogView();
    }

    void LogView()
    {
        List<string> list = LogQueue.Instance.PopAll();
        foreach (string log in list)
            Debug.Log(log);
    }
}