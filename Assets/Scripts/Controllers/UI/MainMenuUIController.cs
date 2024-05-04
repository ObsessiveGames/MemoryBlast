using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : BaseBehaviour {
    [SerializeField] private List<Toggle> difficultyToggleList;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button playButton;

    private SceneLoadingManager sceneLoadingManager => appManager.sceneLoadingManager;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        playButton.onClick.AddListener(OnPlayButtonPressed);
        continueButton.onClick.AddListener(OnContinueButtonPressed);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(OnPlayButtonPressed);
        continueButton.onClick.RemoveListener(OnContinueButtonPressed);
    }

    private void OnContinueButtonPressed() {
        sceneLoadingManager.LoadSceneSingle(Constants.gameScene);
    }

    private void OnPlayButtonPressed() {
        sceneLoadingManager.LoadSceneSingle(Constants.gameScene);
    }
}