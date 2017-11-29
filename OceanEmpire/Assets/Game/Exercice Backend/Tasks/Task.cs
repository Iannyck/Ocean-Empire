using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Task
{
    Reward reward = Reward_Tickets.Build(5);

    public Reward GetReward()
    {
        return reward;
    }
    public abstract ExerciseType GetExerciseType();
    public abstract TimeSpan GetAllocatedTime();

    public override string ToString()
    {
        return "Reward:\n" + reward.ToString();
    }
}
