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

    /// <summary>
    /// Load la scene et propose 3 taches
    /// </summary>
    /// <param name="rewardType"></param>
    public static void ProposeTasks(int rewardType = -1)
    {
        if (Scenes.Exists(SCENENAME))
        {
            Scenes.GetActive(SCENENAME).FindRootObject<InstantExerciseChoice>().Init(rewardType);
        }
        else
        {
            Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
            {
                scene.FindRootObject<InstantExerciseChoice>().Init(rewardType);
            });
        }
    }

    private void Init(int rewardType = -1)
    {
        //TaskBuilder.Build(ExerciseType.Walk)
    }

    public void LaunchExercise_TEMP()
    {
        Scenes.LoadAsync(WaitingWindow.SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            scene.FindRootObject<WaitingWindow>().InitDisplay("Faites une marche de au moins 5 minutes dans votre quartier. Après cette durée" +
                " l'effet dans l'océan sera instantément appliqué", delegate ()
                {
                    RatingWindow.ShowRatingWindow(delegate (HappyRating rating)
                    {
                        // Utiliser le rating pour le rapport
                        if (onCompleteAction != null)
                            onCompleteAction();
                        QuitScene();
                    });
                });
        });
    }
}
