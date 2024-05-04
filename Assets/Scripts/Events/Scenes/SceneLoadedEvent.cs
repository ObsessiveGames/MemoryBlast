using UnityEngine.SceneManagement;

public class SceneLoadedEvent : BaseEvent {
    public Scene scene { get; private set; }
    public LoadSceneMode mode { get; private set; }

    public SceneLoadedEvent(Scene scene, LoadSceneMode mode) {
        this.scene = scene;
        this.mode = mode;
    }
}