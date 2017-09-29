using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToRecolteMessage : SceneMessage
{
    private MapDescription mapDescription;

    public ToRecolteMessage(MapDescription mapDescription)
    {
        this.mapDescription = mapDescription;
    }

    public void OnLoaded(Scene scene)
    {
        Debug.Log("Message transmission");
        //scene.FindRootObject<GameBuilder>().
    }

    public void OnOutroComplete()
    {

    }
}
