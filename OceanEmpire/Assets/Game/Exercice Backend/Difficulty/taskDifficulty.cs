using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class taskDifficulty {

    private static int[] WalkingLevels = { 2, 3, 4, 5, 6, 7, 9, 11, 13, 15, 18, 21, 24, 27, 30, 33, 36, 40, 44, 48, 52, 56, 60 };
    private static int[] RunningLevels = { 2, 3, 4, 5, 6, 7, 9, 11, 13, 15, 18, 21, 24, 27, 30, 33, 36, 40, 44, 48, 52, 56, 60 };
    private static int[] StairsLevels = { 2, 3, 4, 5, 6, 7, 9, 11, 13, 15, 18, 21, 24, 27, 30, 33, 36, 40, 44, 48, 52, 56, 60 };

    public static int MaxLevel
    {
        get { return Mathf.Min(WalkingLevels.Length, RunningLevels.Length, StairsLevels.Length) - 1; }
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

    public static int GetTaskLevel(Task task)
    {
        switch (task.GetExerciseType())
        {
            case ExerciseType.Walk:
                return GetWalkingTaskLevel(task as WalkTask);
            default:
                return -1;
        }
    }

    private static int GetWalkingTaskLevel(WalkTask wTask)
    {
        int minutesOfWalk = ((float)wTask.timeOfWalk.TotalMinutes).RoundedToInt();

        int length = WalkingLevels.Length;
        for (int i = 0; i < length ; ++i)
        {            
            if (minutesOfWalk == WalkingLevels[i])
                return i;
            if (minutesOfWalk < WalkingLevels[i] && i + 2 < length && minutesOfWalk > WalkingLevels[i + 1])
                return i;
        }
        return -1;
    }
}
