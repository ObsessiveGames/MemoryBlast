using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIController : BaseBehaviour {
    [SerializeField] private List<Toggle> difficultyToggleList;
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;

    private SceneLoadingManager sceneLoadingManager => appManager.sceneLoadingManager;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        playButton.onClick.AddListener(OnPlayButtonPressed);
        settingsButton.onClick.AddListener(OnSettingsButtonPressed);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(OnPlayButtonPressed);
        settingsButton.onClick.RemoveListener(OnSettingsButtonPressed);
    }

    private void OnSettingsButtonPressed() {
        
    }

    private void OnPlayButtonPressed() {
        SceneManager.LoadSceneAsync(Constants.gameScene, LoadSceneMode.Single);
        
    }
}