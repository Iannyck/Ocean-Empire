using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward_OceanRefill : Reward
{
    public override bool Apply()
    {
        if (FishPopulation.instance != null)
        {
            FishPopulation.instance.AddRate(1);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override float GetValue()
    {
        return RewardComponents.GetBaseValue(GetRewardType());
    }

    public static Reward_OceanRefill Build()
    {
        return new Reward_OceanRefill();
    }

    public override RewardType GetRewardType()
    {
        return RewardType.OceanRefill;
    }
}
