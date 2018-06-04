using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ToShackMessage : SceneMessage
{
    public FishingReport report;

    public ToShackMessage(FishingReport report)
    {
        this.report = report;
    }

    public void OnLoaded(Scene scene)
    {
        // Donner le report a quelqu'un dans le shack
    }

    public void OnOutroComplete()
    {

    }
}
