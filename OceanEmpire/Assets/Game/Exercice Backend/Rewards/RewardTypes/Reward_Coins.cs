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

<<<<<<< .merge_file_a17232
    public Reward_Coins BuildReverse(int value)
=======
    public static Reward_Coins BuildReverse(int value)
>>>>>>> .merge_file_a10308
    {
        return new Reward_Coins(Mathf.RoundToInt(value / RewardComponents.GetBaseValue(RewardType.Coins)));
    }

<<<<<<< .merge_file_a17232
    public Reward_Coins Build(int coinAmount)
=======
    public static Reward_Coins Build(int coinAmount)
>>>>>>> .merge_file_a10308
    {
        return new Reward_Coins(coinAmount);
    }
}
