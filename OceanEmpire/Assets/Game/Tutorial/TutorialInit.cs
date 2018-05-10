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
    public DataSaver tutorialSaver;

    public bool HasLaunched { get; private set; }

    void Awake()
    {
        if (!tutorialSaver.HasEverLoaded)
            tutorialSaver.Load();
    }

    void Start()
    {
        if (tryLaunchOnStart && !HasLaunched)
            TryToLaunch();
    }

    void Update()
    {
        if (tryLaunchEveryFrame && !HasLaunched)
            TryToLaunch();

        if (tryLaunchOnGameStart && !HasLaunched && Game.Instance != null && Game.Instance.gameStarted)
        {
            TryToLaunch();
            tryLaunchOnGameStart = false;
        }
    }

    public void TryToLaunch()
    {
        if (tutorial.StartCondition())
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
