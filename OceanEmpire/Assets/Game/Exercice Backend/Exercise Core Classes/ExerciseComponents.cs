using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExerciseComponents
{
    public static string GetDisplayName(ExerciseType type)
    {
        switch (type)
        {
            case ExerciseType.Walk:
                return "Marche";
            case ExerciseType.Run:
                return "Course";
            case ExerciseType.Stairs:
                return "Escaliers";
            default:
                return "";
        }
    }
    public static string GetDescription(ExerciseType type)
    {
        switch (type)
        {
            case ExerciseType.Walk:
                return "La marche à pied est une activité physique.";
            case ExerciseType.Run:
                return "La course à pied est une activité physique.";
            case ExerciseType.Stairs:
                return "Montez et descendez les escalier.";
            default:
                return "";
        }
    }

    //Tout autre info/classe relié à un exercice
}
