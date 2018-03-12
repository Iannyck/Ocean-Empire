 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDebug : MonoBehaviour {

    public string mapName;

    void Start ()
    {
        if(SceneManager.sceneCount != 1)
            return;

        PersistentLoader.LoadIfNotLoaded(DebugStartGame);
    }

    void DebugStartGame()
    {
        if (gameObject.scene.name == GameBuilder.SCENENAME)
        {
            OnGameLoaded();
            return;
        }

        Scenes.LoadAsync(GameBuilder.SCENENAME, LoadSceneMode.Additive, OnGameLoaded);
    }

    public void OnGameLoaded(Scene scene)
    {
        scene.FindRootObject<GameBuilder>().Init(mapName);
    }

    public void OnGameLoaded()
    {
        GameBuilder builder = GetComponent<GameBuilder>();
        if (builder != null)
            builder.Init(mapName);
    }
}
