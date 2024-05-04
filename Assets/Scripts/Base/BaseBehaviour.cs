using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour {
    public AppManager appManager { get; private set; }

    private EventManager eventManager => appManager.eventManager;

    public virtual void Setup(AppManager appManager) {
        this.appManager = appManager;
    }

    public void StartListeningToEvent<T>(EventHandler callBack) {
        EventManager eventManagerLocal = eventManager;
        eventManagerLocal.StartListening<T>(callBack);
    }

    public void StopListeningToEvent<T>(EventHandler callBack) {
        eventManager.StopListening<T>(callBack);
    }

    public void TriggerEvent<T>(BaseEvent eventArgs) {
        eventManager.Trigger<T>(eventArgs);
    }
}