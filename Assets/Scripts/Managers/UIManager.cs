using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class UIManager : Manager {
    [SerializeField] private MainMenuUIController mainMenuUIControllerPrefab;
    [SerializeField] private GameUIController gameUIControllerPrefab;

    private MainMenuUIController mainMenuUIController;
    private GameUIController gameUIController;
    private SceneLoadingManager sceneLoadingManager => appManager.sceneLoadingManager;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        StartListeningToEvent<SceneLoadedEvent>(OnSceneLoadedEvent);
    }

    private void OnDestroy() {
        StopListeningToEvent<SceneLoadedEvent>(OnSceneLoadedEvent);
    }

    private void OnSceneLoadedEvent(object sender, EventArgs e) {
        SceneLoadedEvent sceneLoadedEvent = (SceneLoadedEvent)e;
        switch (sceneLoadedEvent.scene.name) {
            // Instantiate UI.
            case mainMenuUIScene:
                sceneLoadingManager.SetActiveScene(mainMenuUIScene);
                mainMenuUIController = Instantiate(mainMenuUIControllerPrefab);
                mainMenuUIController.Setup(appManager);
                break;
            case gameUIScene:
                sceneLoadingManager.SetActiveScene(gameUIScene);
                gameUIController = Instantiate(gameUIControllerPrefab);
                gameUIController.Setup(appManager);
                break;
        }
    }
}