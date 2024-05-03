using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : Manager {
    public GameManager gameManager { get; private set; }
    public SceneLoadingManager sceneLoadingManager { get; private set; }
    public UIManager uiManager { get; private set; }
    public DataManager dataManager { get; private set; }

    [SerializeField] private GameManager gameManagerPrefab;
    [SerializeField] private SceneLoadingManager sceneLoadingManagerPrefab;
    [SerializeField] private UIManager uiManagerPrefab;
    [SerializeField] private DataManager dataManagerPrefab;

    private AudioPlayer audioPlayer;

    private void Awake() {
        Setup(this);
    }

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        DontDestroyOnLoad(this);
        eventManager = Instantiate(eventManagerPrefab);
        sceneLoadingManager = Instantiate(sceneLoadingManagerPrefab);
        uiManager = Instantiate(uiManagerPrefab);
        dataManager = Instantiate(dataManagerPrefab);

        DontDestroyOnLoad(eventManager);
        DontDestroyOnLoad(sceneLoadingManager);
        DontDestroyOnLoad(uiManager);
        DontDestroyOnLoad(dataManager);

        eventManager.Setup(this);
        sceneLoadingManager.Setup(this);
        uiManager.Setup(this);
        dataManager.Setup(this);

        SetupAudioPlayer();

        StartListeningToEvent<SceneLoadedEvent>(OnSceneLoadedEvent);
        sceneLoadingManager.LoadSceneSingle(Constants.mainMenuScene);
    }

    private void OnDestroy() {
        StopListeningToEvent<SceneLoadedEvent>(OnSceneLoadedEvent);
    }

    private void OnSceneLoadedEvent(object sender, EventArgs e) {
        SceneLoadedEvent sceneLoadedEvent = (SceneLoadedEvent)e;
        switch (sceneLoadedEvent.scene.name) {
            case Constants.mainMenuScene:
                sceneLoadingManager.LoadSceneAdditive(Constants.mainMenuUIScene);
                break;
            case Constants.gameScene:
                SetupGameManager();
                sceneLoadingManager.LoadSceneAdditive(Constants.gameUIScene);
                break;
        }
    }

    private void SetupGameManager() {
        gameManager = Instantiate(gameManagerPrefab);
        DontDestroyOnLoad(gameManager);
        gameManager.Setup(this);
    }

    private void SetupAudioPlayer() {
        audioPlayer = Instantiate(Resources.Load<AudioPlayer>(Constants.audioPlayerPath));
        DontDestroyOnLoad(audioPlayer);
    }
}