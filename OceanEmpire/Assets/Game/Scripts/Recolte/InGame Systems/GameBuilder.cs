
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
        if (!Scenes.IsActiveOrBeingLoaded(gameSettings.MapScene))
            Scenes.LoadAsync(gameSettings.MapScene, LoadSceneMode.Additive, OnMapLoaded);
        else
            OnMapLoaded();

        if (!Scenes.IsActiveOrBeingLoaded(Recolte_UI.SCENENAME))
            Scenes.LoadAsync(Recolte_UI.SCENENAME, LoadSceneMode.Additive, OnUILoaded);
        else
            OnUILoaded();
    }

    void OnMapLoaded(Scene scene)
    {
        Game.Instance.SetReference(scene.FindRootObject<MapInfo>());
        Game.Instance.SetReference(scene.FindRootObject<FishLottery>());

        mapLoaded = true;
        CheckInitGame();
    }

    void OnMapLoaded()
    {
        OnMapLoaded(SceneManager.GetSceneByName(gameSettings.MapScene));
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
