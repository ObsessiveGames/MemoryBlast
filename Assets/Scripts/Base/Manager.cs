using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : BaseBehaviour {
    [SerializeField] protected EventManager eventManagerPrefab;
    protected GameObject eventManagerObject;
    public EventManager eventManager { get; protected set; }
}