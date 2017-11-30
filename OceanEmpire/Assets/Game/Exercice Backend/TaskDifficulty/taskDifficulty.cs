using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskDifficulty{

    private static int[] WalkingLevels = { 3, 4, 5, 6, 7, 9, 11, 13, 15, 18, 21, 24, 27, 30, 33, 36, 40, 44, 48, 52, 56, 60 };
    private static int[] RunningLevels = { 3, 4, 5, 6, 7, 9, 11, 13, 15, 18, 21, 24, 27, 30, 33, 36, 40, 44, 48, 52, 56, 60 };
    private static int[] StairsLevels = { 3, 4, 5, 6, 7, 9, 11, 13, 15, 18, 21, 24, 27, 30, 33, 36, 40, 44, 48, 52, 56, 60 };

    public static int MaxLevel
    {
         get { return Mathf.Min(WalkingLevels.Length, RunningLevels.Length, StairsLevels.Length) - 1;}
    }

public static int GetExerciseDifficulty(ExerciseType type, int level)
    {
        if (level > MaxLevel)
            return -1;

        switch (type)
        {
            case ExerciseType.Walk:
                return WalkingLevels[level];
            case ExerciseType.Run:
                return RunningLevels[level];
            case ExerciseType.Stairs:
                return StairsLevels[level];
            default:
                return -1;
        }     
    }
}
