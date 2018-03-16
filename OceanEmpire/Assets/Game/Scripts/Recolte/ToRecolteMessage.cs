using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ToRecolteMessage : SceneMessage
{
    private GameSettings gameSettings;

    public ToRecolteMessage(GameSettings gameSettings)
    {
        this.gameSettings = gameSettings;
    }

    public void OnLoaded(Scene scene)
    {
        scene.FindRootObject<GameBuilder>().Init(gameSettings);
    }

    public void OnOutroComplete()
    {

    }
}
