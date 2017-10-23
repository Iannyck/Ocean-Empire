using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe statique qui construit des tache
/// </summary>
public static class TaskBuilder
{
    public static Task Build(ExerciseType type, float difficulty)
    {
        float rand = Random.value;
        switch (type)
        {
            case ExerciseType.Walk:
                {
                    float i = 1f / 2f;
                    if (i < rand)
                    {
                        return WalkTask_Distance.Build(difficulty);
                    }
                    else
                    {
                        return WalkTask_Duration.Build(difficulty);
                    }
                }
            case ExerciseType.Run:
                return null;
            case ExerciseType.Stairs:
                return null;
            default:
                return null;
        }
    }
}
