using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBuilder : MonoBehaviour {

    public const string SCENENAME = "GameBuilder";

    [HideInInspector]
    string mapName = "Map_Caraibe";
    bool mapLoaded = false;
    bool uiLoaded = false;

    public void Init(string mapName)
    {
        this.mapName = mapName;
        MasterManager.Sync(Build);
    }

    void Build()
    {
        // Load All Scenes
        if (!Scenes.Exists(mapName))
            Scenes.LoadAsync(mapName, LoadSceneMode.Additive, OnMapLoaded);
        else
            OnMapLoaded();

        if (!Scenes.Exists(Recolte_UI.SCENENAME))
            Scenes.LoadAsync(Recolte_UI.SCENENAME, LoadSceneMode.Additive, OnUILoaded);
        else
            OnUILoaded();
    }

    void OnMapLoaded(Scene scene)
    {
        Game.instance.map = scene.FindRootObject<MapInfo>();

        mapLoaded = true;
        CheckInitGame();
    }

    void OnMapLoaded()
    {
        OnMapLoaded(SceneManager.GetSceneByName(mapName));
    }

    void OnUILoaded(Scene scene)
    {
        Game.instance.ui = scene.FindRootObject<Recolte_UI>();

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
            Game.instance.InitGame();
        }
    }
}
