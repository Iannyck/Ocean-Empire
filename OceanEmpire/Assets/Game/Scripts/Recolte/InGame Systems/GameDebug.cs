using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDebug : MonoBehaviour
{
    public PrebuiltMapData map;

    void Start()
    {
        if (SceneManager.sceneCount != 1)
            return;

        PersistentLoader.LoadIfNotLoaded(DebugStartGame);
    }

    void DebugStartGame()
    {
        if (gameObject.scene.name == GameBuilder.SCENENAME)
        {
            OnGameLoaded(gameObject.scene);
            return;
        }

        Scenes.LoadAsync(GameBuilder.SCENENAME, LoadSceneMode.Additive, OnGameLoaded);
    }

    public void OnGameLoaded(Scene scene)
    {
        GameSettings gameSettings = new GameSettings
        (
            mapScene: map.MapData.GameSceneName,
            canUseFishingFrenzy: false
        );
        scene.FindRootObject<GameBuilder>().Init(gameSettings);
    }
}
