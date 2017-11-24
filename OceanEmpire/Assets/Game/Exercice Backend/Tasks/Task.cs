using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    Reward reward = Reward_Tickets.Build(5);

    public Reward GetReward()
    {
        return reward;
    }
    public abstract ExerciseType GetExerciseType();
}
