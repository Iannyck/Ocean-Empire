
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBuilder : MonoBehaviour
{
    public const string SCENENAME = "GameBuilder";
    
    bool mapLoaded = false;
    bool uiLoaded = false;
    GameSettings gameSettings;

    public void Init(GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;
        PersistentLoader.LoadIfNotLoaded(Build);
    }

    void Build()
    {
        // Load All Scenes
        if (!Scenes.IsActiveOrBeingLoaded(gameSettings.MapData.GameSceneName))
            Scenes.LoadAsync(gameSettings.MapData.GameSceneName, LoadSceneMode.Additive, OnMapLoaded);
        else
            OnMapLoaded();

        if (!Scenes.IsActiveOrBeingLoaded(Recolte_UI.SCENENAME))
            Scenes.LoadAsync(Recolte_UI.SCENENAME, LoadSceneMode.Additive, OnUILoaded);
        else
            OnUILoaded();
    }

    void OnMapLoaded(Scene scene)
    {
        //Game.Instance.SetReference(scene.FindRootObject<MapLayout>());
        Game.Instance.SetReference(scene.FindRootObject<FishLottery>());
        //Game.Instance.SetReference(scene.FindRootObject<MapBuilder>());
        //Game.Instance.SetReference(scene.FindRootObject<Shack_Environment>());

        mapLoaded = true;
        CheckInitGame();
    }

    void OnMapLoaded()
    {
        OnMapLoaded(SceneManager.GetSceneByName(gameSettings.MapData.GameSceneName));
    }

    void OnUILoaded(Scene scene)
    {
        Game.Instance.SetReference(scene.FindRootObject<Recolte_UI>());

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
