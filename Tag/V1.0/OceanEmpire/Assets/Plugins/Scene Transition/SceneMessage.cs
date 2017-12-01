using UnityEngine.SceneManagement;

public interface SceneMessage
{
    void OnLoaded(Scene scene);
    void OnOutroComplete();
}
