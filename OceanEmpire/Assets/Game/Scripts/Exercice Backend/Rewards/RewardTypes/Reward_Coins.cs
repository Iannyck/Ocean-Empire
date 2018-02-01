using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward_Coins : Reward
{
    public int amount;

    public override bool Apply()
    {
        return PlayerCurrency.AddCoins(amount);
    }

    public override MarketValue GetMarketValue()
    {
        return amount * RewardComponents.GetBaseValue(GetRewardType());
    }

    public int GetAmount()
    {
        return amount;
    }

    private Reward_Coins(int coins)
    {
        amount = coins;
    }

    public override RewardType GetRewardType()
    {
        return RewardType.Coins;
    }

    public static Reward_Coins BuildFromValue(float value)
    {
        return new Reward_Coins(Mathf.RoundToInt(value / RewardComponents.GetBaseValue(RewardType.Coins).floatValue));
    }

    public static Reward_Coins BuildFromAmount(int coinAmount)
    {
        return new Reward_Coins(coinAmount);
    }

    public override string ToString()
    {
        return amount + " coins";
    }
}
