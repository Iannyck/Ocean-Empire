using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using CCC.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InstantExerciseChoice : WindowAnimation
{
    public const string SCENENAME = "InstantExercise";

    public InstantExerciseChoice_Item[] taskDisplays;

    /// <summary>
    /// Load la scene et propose 3 taches
    /// </summary>
    /// <param name="rewardType"></param>
    public static void ProposeTasks(RewardType rewardType)
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

    protected override void Start()
    {
        base.Start();
        if (Scenes.SceneCount == 1)
            ProposeTasks(RewardType.Tickets);
    }

    private void OnItemClick(InstantExerciseChoice_Item item)
    {
        //item.assignedTask
        ConfirmInstantExercise.OpenWindowAndConfirm(item.assignedTask, (hasConfirmed) =>
        {
            if (hasConfirmed)
            {
                InstantTask instantTask = new InstantTask(item.assignedTask);
                if (Calendar.instance.AddScheduledTask(instantTask))
                {
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

    private void Init(RewardType rewardType)
    {
        int taskCount = taskDisplays.Length;

        List<Task> tasks;

        if (rewardType == RewardType.OceanRefill)
        {
            Montant refillCost = RefillCost.GetRefillCost();
            print(refillCost.amount);
            int level = RewardBuilder.RevertRewardByLevel(refillCost.amount);

            Task task = TaskBuilder.Build(ExerciseType.Walk, taskDifficulty.GetExerciseDifficulty(ExerciseType.Walk, level));
            task.SetReward(RewardType.OceanRefill, 0);
            tasks = new List<Task>();
            tasks.Add(task);
            taskCount = 1;
        }
        else
            tasks = ExercisePropositionMaker.GetExercisePropositions(taskCount, rewardType);

        int i = 0;
        for (i = 0; i < taskCount; i++)
        {
            taskDisplays[i].DisplayTask(tasks[i]);
        }

        for (; i < taskDisplays.Length; i++)
        {
            taskDisplays[i].DestroyGO();
        }
        /*
        int taskCount = taskDisplays.Length;
        float difficultyStart = 0;
        float difficultyEnd = 1;
        float increment = (difficultyEnd - difficultyStart) / taskCount;

        float currentDifficulty = difficultyStart;
        for (int i = 0; i < taskCount; i++)
        {
            taskDisplays[i].DisplayTask(TaskBuilder.Build(ExerciseType.Walk, currentDifficulty));
            currentDifficulty += increment;
        }*/
    }
}
