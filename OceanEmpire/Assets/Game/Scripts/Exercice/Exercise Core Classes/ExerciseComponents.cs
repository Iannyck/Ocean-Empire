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
            case ExerciseType.PressKey:
                return "Appuyer sur une touche";
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
            case ExerciseType.PressKey:
                return "Appuyez sur une touche du clavier pour faire de l'activité physique !";
            default:
                return "";
        }
    }

    public static List<ExerciseType> GetAllTypes()
    {
        return new List<ExerciseType>()
        {
            ExerciseType.Walk,
            ExerciseType.Run,
            ExerciseType.Stairs,
            ExerciseType.PressKey
        };
    }
}
