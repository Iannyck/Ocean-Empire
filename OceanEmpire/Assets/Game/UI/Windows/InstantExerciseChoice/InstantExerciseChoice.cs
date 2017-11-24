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
        ConfirmInstantExercise.OpenWindowAndConfirm(item.assignedTask, (hasConfirmed) =>
        {
            if (hasConfirmed)
                Calendar.instance.AddScheduledTask(new InstantTask(item.assignedTask));
            print(hasConfirmed);
        });
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
}
