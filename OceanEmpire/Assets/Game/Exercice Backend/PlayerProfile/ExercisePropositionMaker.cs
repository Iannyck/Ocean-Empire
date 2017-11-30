using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExercisePropositionMaker{

    public static List<Task> GetExercisePropositions(int amount)
    {
        List<int> levels = getTaskLevels(amount);
        List<ExerciseType> types = GetTypes(amount);

        List<Task> tasks = new List<Task>();

        for (int i = 0; i < amount; ++i)
            tasks.Add(TaskBuilder.Build(types[i], TaskDifficulty.GetExerciseDifficulty(types[i], levels[i])));

        return tasks;
    }
   

    private static List<int> getTaskLevels(int amount)
    {
        Lottery<int> levelsLottery;

        levelsLottery = new Lottery<int>(TaskDifficulty.MaxLevel + 1);
        for (int i = 0; i < TaskDifficulty.MaxLevel; ++i)
        {
            levelsLottery.Add(i, GetWeigth(i));
        }

        if (amount > TaskDifficulty.MaxLevel + 1)
            amount = TaskDifficulty.MaxLevel + 1;

        List<int> difficulties = new List<int>(); 
        while(difficulties.Count < amount )
        {
            int difPicked = levelsLottery.Pick();
            if (difficulties.Contains(difPicked) == false)
                difficulties.Add(difPicked);
        }

        difficulties.Sort();
        return difficulties;
    }

    private static float GetWeigth(int checkedLevel)
    {
        const float declineRate = 2f;    //pour ure rate de 2 et un écart n au niveau du joueur, le poids est de 1/(2^n)
        int playerLevel = PlayerProfile.Level;
        float weigth = 1 / Mathf.Pow(declineRate, Mathf.Abs(playerLevel - checkedLevel) );
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
