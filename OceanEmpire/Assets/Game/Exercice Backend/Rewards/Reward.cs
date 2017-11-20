using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward
{
    public abstract void Apply();
    public abstract RewardType GetRewardType();
    public abstract float GetValue();
}
