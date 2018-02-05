using System.Collections.Generic;
using UnityEngine;
using System;

public class PrioritySheet : ScriptableObject {

    public enum ExerciseTypes
    {
        walk = 0,
        run = 1,
        bicycle = 2
    }

    [SerializeField,Reorderable,Header("Plus la priorité est haute plus c'est prioritaire")] public List<ExerciseTypes> Exercices = new List<ExerciseTypes>();

    /// <summary>
    /// Retourne le plus prioritaire des deux : -1 (error), 0 (first), 1(second), 2(egale)
    /// </summary>
    public int Compare(ExerciseTypes first, ExerciseTypes second)
    {
        int firstValue = FindExerciceValueByType(first);
        int secondValue = FindExerciceValueByType(second);

        if (firstValue < 0 || secondValue < 0)
            return -1;
        else if (firstValue > secondValue)
            return 0;
        else if (firstValue < secondValue)
            return 1;
        else
            return 2;
    }

    private int FindExerciceValueByType(ExerciseTypes type)
    {
        for (int i = 0; i < Exercices.Count; i++)
        {
            if (Exercices[i] == type)
                return i;
        }
        return -1;
    }

    public string ExerciseTypeToString(ExerciseTypes type)
    {
        switch (type)
        {
            case ExerciseTypes.walk:
                return "walk";
            case ExerciseTypes.run:
                return "run";
            case ExerciseTypes.bicycle:
                return "bicycle";
            default:
                return "";
        }
    }
}
