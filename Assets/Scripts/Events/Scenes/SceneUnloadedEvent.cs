using UnityEngine.SceneManagement;

public class SceneUnloadedEvent : BaseEvent {
    public Scene scene { get; private set; }

    public SceneUnloadedEvent(Scene scene) {
        this.scene = scene;
    }
}