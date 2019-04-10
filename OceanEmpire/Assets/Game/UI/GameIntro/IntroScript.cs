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

    public DataSaver firstRunSaver;
    

    //private const string firstRunKey = "firstRun";

    void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(Go);
        NotificationManager.Cancel(4242);
        NotificationManager.SendWithAppIcon(new TimeSpan(0, 7, 0, 0, 0), "test", "Lancement d'ocean empire", Color.blue, id:4242);

    }

    void Go()
    {
        
        firstRunSaver.LoadAsync(delegate ()
        {
            var sq = DOTween.Sequence();
            sq.Append(uqacLogo.DOFade(1, animDuration));
            sq.Append(uqacLogo.DOFade(0, animDuration));
            sq.OnComplete(delegate ()
            {
                // FIRST_QUESTS_GIVEN ?
                if (firstRunSaver.GetBool(FirstQuestsGiver.FIRST_QUESTS_GIVEN, false))
                {
                    Scenes.Load(shack);
                }
                else
                {
                    //tutorialSaver.SetBool(firstRunKey, false);
                    //tutorialSaver.Save();

                    Scenes.Load(tutorialMap);
                }
            });
        });
    }
}
