using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ToRecolteMessage : SceneMessage
{
    private string mapScene;
    private GameSettings gameSettings;

    public ToRecolteMessage(string mapScene, GameSettings gameSettings)
    {
        this.mapScene = mapScene;
        this.gameSettings = gameSettings;
    }

    public void OnLoaded(Scene scene)
    {
        scene.FindRootObject<GameBuilder>().Init(mapScene, gameSettings);
    }

    public void OnOutroComplete()
    {

    }
}
