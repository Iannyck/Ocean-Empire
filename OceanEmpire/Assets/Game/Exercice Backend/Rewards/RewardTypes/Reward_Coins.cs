using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_Coins : Reward
{
    public int amount;

    public override void Apply()
    {
    }

    public override float GetValue()
    {
        return amount * RewardComponents.GetBaseValue(GetRewardType());
    }

    private Reward_Coins(int coins)
    {
        amount = coins;
    }

    public override RewardType GetRewardType()
    {
        return RewardType.Coins;
    }

    public Reward_Coins BuildReverse(int value)
    {
        return new Reward_Coins(Mathf.RoundToInt(value / RewardComponents.GetBaseValue(RewardType.Coins)));
    }

    public Reward_Coins Build(int coinAmount)
    {
        return new Reward_Coins(coinAmount);
    }
}
