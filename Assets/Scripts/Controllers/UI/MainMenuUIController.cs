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
    private DataManager dataManager => appManager.dataManager;

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        if (dataManager.data.matchTurn > 0) {
            continueButton.interactable = true;
            continueButton.onClick.AddListener(OnContinueButtonPressed);
        } else {
            continueButton.interactable = false;
        }
        playButton.onClick.AddListener(OnPlayButtonPressed);
    }

    private void OnDestroy() {
        playButton.onClick.RemoveListener(OnPlayButtonPressed);
        if (continueButton.gameObject.activeSelf) continueButton.onClick.RemoveListener(OnContinueButtonPressed);
    }

    private void OnContinueButtonPressed() {
        dataManager.IsPreviousGame(true);
        sceneLoadingManager.LoadSceneSingle(Constants.gameScene);
    }

    private void OnPlayButtonPressed() {
        dataManager.IsPreviousGame(false);
        sceneLoadingManager.LoadSceneSingle(Constants.gameScene);
    }
}