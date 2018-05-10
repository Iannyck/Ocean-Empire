using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Questing;

public class IntroScript : MonoBehaviour
{

    public Image uqacLogo;
    public float animDuration;
    public SceneInfo tutorialMap;
    public SceneInfo shack;
    public DataSaver tutorialSaver;


    private const string firstRunKey = "firstRun";

    void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(Go);
    }

    void Go()
    {
        tutorialSaver.LoadAsync(delegate ()
        {
            var sq = DOTween.Sequence();
            sq.Append(uqacLogo.DOFade(1, animDuration));
            sq.Append(uqacLogo.DOFade(0, animDuration));
            sq.OnComplete(delegate ()
            {
                // First run
                if (tutorialSaver.GetBool(firstRunKey, true))
                {
                    tutorialSaver.SetBool(firstRunKey, false);
                    tutorialSaver.Save();

                    Scenes.Load(tutorialMap);
                }
                else
                {
                    Scenes.Load(shack);
                }
            });
        });
    }
}
