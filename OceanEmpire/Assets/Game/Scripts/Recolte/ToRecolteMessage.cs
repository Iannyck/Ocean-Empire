using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ToRecolteMessage : SceneMessage
{
    private string mapScene;

    public ToRecolteMessage(string mapScene)
    {
        this.mapScene = mapScene;
    }

    public void OnLoaded(Scene scene)
    {
        scene.FindRootObject<GameBuilder>().Init(mapScene);
    }

    public void OnOutroComplete()
    {

    }
}
