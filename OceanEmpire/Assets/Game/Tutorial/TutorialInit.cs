using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class TutorialInit : MonoBehaviour
{
    public bool tryLaunchOnStart = true;
    public bool tryLaunchOnGameStart = false;
    public bool tryLaunchEveryFrame = false;
    public BaseTutorial tutorial;

    public bool HasLaunched { get; private set; }

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if (tryLaunchOnStart)
                TryToLaunch();
        });
    }

    void Update()
    {
        if (tryLaunchEveryFrame)
            TryToLaunch();

        if (tryLaunchOnGameStart && Game.Instance != null && Game.Instance.gameStarted)
        {
            TryToLaunch();
            tryLaunchOnGameStart = false;
        }
    }

    public void TryToLaunch()
    {
        if (HasLaunched)
            return;

        if (tutorial.LaunchCondition())
        {
            Launch();
        }
    }

    private void Launch()
    {
        if (HasLaunched)
        {
            Debug.LogError("Cannot launch a tutorial twice");
            return;
        }

        HasLaunched = true;
        TutorialScene.StartTutorial(tutorial.name, tutorial.dataSaver);
    }
}
