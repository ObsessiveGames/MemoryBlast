using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class EventManager : Manager {
    private Dictionary<Type, EventHandlerCapsule> eventsByType;

    private EventHandlerCapsule EventHandlerCapsuleFactory() {
        return new EventHandlerCapsule();
    }

    public override void Setup(AppManager appManager) {
        base.Setup(appManager);
        SetupEvents();
    }

    public void StartListening<T>(EventHandler callBack) {
        EventHandlerCapsule thisEvent = null;
        if (eventsByType.TryGetValue(typeof(T), out thisEvent)) {
            thisEvent.thisEvent += callBack;
        }
    }

    public void StopListening<T>(EventHandler callBack) {
        EventHandlerCapsule thisEvent = null;
        if (eventsByType.TryGetValue(typeof(T), out thisEvent)) {
            thisEvent.thisEvent -= callBack;
        }
    }

    public void Trigger<T>(BaseEvent eventArgs) {
        EventHandlerCapsule thisEvent = null;
        if (eventsByType.TryGetValue(typeof(T), out thisEvent)) {
            if (thisEvent.thisEvent != null) {
                thisEvent.thisEvent(this, eventArgs);
            }
        }
    }

    private void SetupEvents() {
        List<Type> types = FindDerivedTypes(Assembly.GetCallingAssembly(), typeof(BaseEvent));
        eventsByType = new Dictionary<Type, EventHandlerCapsule>();
        foreach (Type t in types) {

            eventsByType.Add(t, EventHandlerCapsuleFactory());
        }
    }

    private List<Type> FindDerivedTypes(Assembly assembly, Type baseType) {

        List<Type> allTypes = assembly.GetTypes().ToList<Type>();
        List<Type> types = new List<Type>();
        foreach (Type t in allTypes) {
            if (t.IsSubclassOf(baseType)) {
                types.Add(t);
            }
        }
        return types;
    }
}
