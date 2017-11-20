using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_OceanRefill : Reward
{
    public override void Apply()
    {
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
