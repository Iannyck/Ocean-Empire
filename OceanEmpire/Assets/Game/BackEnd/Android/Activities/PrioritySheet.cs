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

    [Serializable]
	public class ExerciseValue
    {
        public ExerciseTypes type;
        public float priority;
    }

    [SerializeField,Reorderable] public List<ExerciseValue> Exercices = new List<ExerciseValue>();

    /// <summary>
    /// Retourne le plus prioritaire des deux : -1 (error), 0 (first), 1(second), 2(egale)
    /// </summary>
    public int Compare(ExerciseTypes first, ExerciseTypes second)
    {
        ExerciseValue firstValue = FindExerciceValueByType(first);
        ExerciseValue secondValue = FindExerciceValueByType(second);

        if (firstValue == null || secondValue == null)
            return -1;
        else if (firstValue.priority > secondValue.priority)
            return 0;
        else if (firstValue.priority < secondValue.priority)
            return 1;
        else
            return 2;
    }

    private ExerciseValue FindExerciceValueByType(ExerciseTypes type)
    {
        for (int i = 0; i < Exercices.Count; i++)
        {
            if (Exercices[i].type == type)
                return Exercices[i];
        }
        return null;
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
