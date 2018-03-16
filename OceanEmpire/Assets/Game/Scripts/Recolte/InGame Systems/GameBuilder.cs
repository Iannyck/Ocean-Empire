
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBuilder : MonoBehaviour
{
    public const string SCENENAME = "GameBuilder";

    string mapSceneName;
    bool mapLoaded = false;
    bool uiLoaded = false;
    GameSettings gameSettings;

    public void Init(string mapScene, GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;
        mapSceneName = mapScene;
        PersistentLoader.LoadIfNotLoaded(Build);
    }

    void Build()
    {
        // Load All Scenes
        if (!Scenes.IsActiveOrBeingLoaded(mapSceneName))
            Scenes.LoadAsync(mapSceneName, LoadSceneMode.Additive, OnMapLoaded);
        else
            OnMapLoaded();

        if (!Scenes.IsActiveOrBeingLoaded(Recolte_UI.SCENENAME))
            Scenes.LoadAsync(Recolte_UI.SCENENAME, LoadSceneMode.Additive, OnUILoaded);
        else
            OnUILoaded();
    }

    void OnMapLoaded(Scene scene)
    {
        Game.Instance.map = scene.FindRootObject<MapInfo>();
        Game.Instance.fishLottery = scene.FindRootObject<FishLottery>();

        mapLoaded = true;
        CheckInitGame();
    }

    void OnMapLoaded()
    {
        OnMapLoaded(SceneManager.GetSceneByName(mapSceneName));
    }

    void OnUILoaded(Scene scene)
    {
        Game.Instance.ui = scene.FindRootObject<Recolte_UI>();

        uiLoaded = true;
        CheckInitGame();
    }

    void OnUILoaded()
    {
        OnUILoaded(SceneManager.GetSceneByName(Recolte_UI.SCENENAME));
    }

    void CheckInitGame()
    {
        if (uiLoaded && mapLoaded)
        {
            Game.Instance.InitGame(gameSettings);
        }
    }
}
