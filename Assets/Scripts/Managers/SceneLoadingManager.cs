using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : Manager {
    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene) {
        TriggerEvent<SceneUnloadedEvent>(new SceneUnloadedEvent(scene));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        TriggerEvent<SceneLoadedEvent>(new SceneLoadedEvent(scene, mode));
    }

    public void LoadSceneSingle(string name) {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void LoadSceneAdditive(string name) {
        if (SceneManager.GetSceneByName(name).IsValid()) {
            //Debug.LogError($"Why are we trying to load {name} as a duplicate scene?");
            return;
        } else {
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
    }

    public void UnLoadScene(string name) {
        SceneManager.UnloadSceneAsync(name);
    }
    public void SetActiveScene(string name) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}