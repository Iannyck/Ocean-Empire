using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantExerciseChoice : WindowAnimation
{
    public const string SCENENAME = "InstantExercise";

    public GameObject[] rewardDisplays;

    private Action onCompleteAction;

    public void Init(bool showRewards, Action action)
    {
        this.onCompleteAction = action;

        if (!showRewards)
        {
            for (int i = 0; i < rewardDisplays.Length; i++)
            {
                rewardDisplays[i].SetActive(false);
            }
        }
    }

    public void LaunchExercise_TEMP()
    {
        Scenes.LoadAsync(WaitingWindow.SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            scene.FindRootObject<WaitingWindow>().InitDisplay("Faites une marche de au moins 5 minutes dans votre quartier. Après cette durée" +
                " l'effet dans l'océan sera instantément appliqué", delegate ()
                {
                    if (onCompleteAction != null)
                        onCompleteAction();
                    QuitScene();
                });
        });
    }
}
