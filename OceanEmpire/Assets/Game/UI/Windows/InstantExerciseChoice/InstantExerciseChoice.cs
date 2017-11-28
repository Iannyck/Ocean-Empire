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

        ActivityDetection.ResetActivitiesSave();

        for (int i = 0; i < taskDisplays.Length; i++)
        {
            taskDisplays[i].onClick = OnItemClick;
        }
    }

    protected override void Start()
    {
        base.Start();
        if (Scenes.SceneCount == 1)
            ProposeTasks();
    }

    private void OnItemClick(InstantExerciseChoice_Item item)
    {
        //item.assignedTask
        ConfirmInstantExercise.OpenWindowAndConfirm(item.assignedTask, (hasConfirmed) =>
        {
            print("Has confirmed: " + hasConfirmed);
            if (hasConfirmed)
            {
                InstantTask instantTask = new InstantTask(item.assignedTask);
                if (Calendar.instance.AddScheduledTask(instantTask))
                {

                    TrackingWindow.ShowWaitingWindow("", instantTask);
                    Close();
                }
                else
                {
                    MessagePopup.DisplayMessage("La plage horaire est d" + TextCharacters.E_Aigue + "j" + TextCharacters.A_Grave +
                        " occup" + TextCharacters.E_Aigue +
                        " par un autre exercice.");
                }
            }
        });
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
}
