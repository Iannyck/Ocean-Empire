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

    [Header("Start quests")]
    public PrebuiltMapData firstMap;


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

                    if (firstMap)
                    {
                        var now = DateTime.Now;
                        foreach (var quest in firstMap.GetRelatedQuestBuilders())
                        {
                            if (quest != null)
                                QuestManager.Instance.AddQuest(quest.BuildQuest(now), false);
                        }
                        QuestManager.Instance.LateSave();
                    }

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
