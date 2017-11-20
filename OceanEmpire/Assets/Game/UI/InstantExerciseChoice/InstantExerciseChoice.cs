using CCC.Manager;
using CCC.UI;
using System;
using UnityEngine.SceneManagement;

public class InstantExerciseChoice : WindowAnimation
{
    public const string SCENENAME = "InstantExercise";

    public InstantExerciseChoice_Item[] taskDisplays;

    private Action onCompleteAction;

    /// <summary>
    /// Load la scene et propose 3 taches
    /// </summary>
    /// <param name="rewardType"></param>
    public static void ProposeTasks(int rewardType = -1)
    {
        MasterManager.Sync(() =>
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
        });
    }

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < taskDisplays.Length; i++)
        {
            taskDisplays[i].onClick = OnItemClick;
        }
    }

    private void OnItemClick(InstantExerciseChoice_Item item)
    {
        //item.assignedTask
        print("touch: " + item.transform.GetSiblingIndex());
    }

    private void Init(int rewardType = -1)
    {
        int taskCount = taskDisplays.Length;
        float difficultyStart = 0;
        float difficultyEnd = 1;
        float increment = (difficultyEnd - difficultyStart) / taskCount;

        float currentDifficulty = difficultyStart;
        for (int i = 0; i < taskCount; i++)
        {
            taskDisplays[i].DisplayTask(TaskBuilder.Build(ExerciseType.Walk, currentDifficulty));
            currentDifficulty += increment;
        }
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
