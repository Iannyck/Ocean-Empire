using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExercisePropositionMaker
{

    //Influence la la largeur des options offerte par rapport au niveau du joueur
    //pour ure rate de 2 et un exercice avec un écart de niveau n au par rapport au joueur, le poids est de 1/(2^n)
    const float declineRate = 2f;

    public static List<Task> GetExercisePropositions(int amount, RewardType rewardType)
    {
        List<int> levels = GetTaskLevels(amount);
        List<ExerciseType> types = GetTypes(amount);

        List<Task> tasks = new List<Task>();

        for (int i = 0; i < amount; ++i)
        {
            Task newTask = TaskBuilder.Build(types[i], taskDifficulty.GetExerciseDifficulty(types[i], levels[i]));
            newTask.SetAutoReward(rewardType);
            tasks.Add(newTask);
        }
        return tasks;
    }

    private static List<int> GetTaskLevels(int amount)
    {
        /*
        Lottery<int> levelsLottery;

        levelsLottery = new Lottery<int>(taskDifficulty.MaxLevel + 1);
        for (int i = 0; i < taskDifficulty.MaxLevel; ++i)
        {
            levelsLottery.Add(i, GetWeigth(i));
        }

        if (amount > taskDifficulty.MaxLevel + 1)
            amount = taskDifficulty.MaxLevel + 1;

        List<int> difficulties = new List<int>(); 
        while(difficulties.Count < amount )
        {
            int difPicked = levelsLottery.Pick();
            if (difficulties.Contains(difPicked) == false)
                difficulties.Add(difPicked);
        }
        
        difficulties.Sort();
        */

        List<int> difficulties = new List<int>();
        int startValue = Mathf.Max(PlayerProfile.Level - ((amount - 1) / 2.0f).RoundedToInt(), 0);
        int maxValue = startValue + amount;
        for (int i = startValue; i <= maxValue; ++i)
            difficulties.Add(i);

        return difficulties;
    }

    private static float GetWeigth(int checkedLevel)
    {
        int playerLevel = PlayerProfile.Level;
        float weigth = 1 / Mathf.Pow(declineRate, Mathf.Abs(playerLevel - checkedLevel));
        return weigth;
    }


    private static List<ExerciseType> GetTypes(int amount)
    {
        List<ExerciseType> types = new List<ExerciseType>();
        for (int i = 0; i < amount; ++i)
            types.Add(ExerciseType.Walk);
        return types;
    }
}
