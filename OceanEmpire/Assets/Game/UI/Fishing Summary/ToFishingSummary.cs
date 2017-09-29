using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct ToFishingSummaryMessage : SceneMessage
{
    public FishingReport report;

    public ToFishingSummaryMessage(FishingReport report)
    {
        this.report = report;
    }

    public void OnLoaded(Scene scene)
    {
        scene.FindRootObject<FishingSummary>().ShowReport(report);
    }

    public void OnOutroComplete()
    {
    }
}
