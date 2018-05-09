using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class TutorialInit : MonoBehaviour
{

    public bool onStart = false;
    public bool onGameReady = true;
    public BaseTutorial tutorial;
    public DataSaver tutorialSaver;

    private bool hasBeenInit = false;

    void Start()
    {
        if (onStart)
            Init();
    }

    void Update()
    {
        if (onGameReady)
        {
            if (!hasBeenInit)
            {
                if (Game.Instance != null)
                {
                    Game.OnGameReady += Init;
                    hasBeenInit = true;
                }
            }
        }
    }

    private void Init()
    {
        if (!tutorialSaver.HasEverLoaded)
        {
            tutorialSaver.Load(delegate ()
            {
                TutorialScene.StartTutorial(tutorial.name, tutorial.dataSaver);
            });
        }
        else
        {
            TutorialScene.StartTutorial(tutorial.name, tutorial.dataSaver);
        }
    }
}
