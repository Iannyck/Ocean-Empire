using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward_Tickets : Reward
{
    public int amount;

    public override bool Apply()
    {
        return PlayerCurrency.AddTickets(amount);
    }

    public override MarketValue GetMarketValue()
    {
        return amount * RewardComponents.GetBaseValue(GetRewardType());
    }

    public int GetAmount()
    {
        return amount;
    }

    private Reward_Tickets(int tickets)
    {
        amount = tickets;
    }

    public override RewardType GetRewardType()
    {
        return RewardType.Tickets;
    }

    public static Reward_Tickets BuildFromValue(float value)
    {
        return new Reward_Tickets(Mathf.RoundToInt(value / RewardComponents.GetBaseValue(RewardType.Tickets).floatValue));
    }

    public static Reward_Tickets BuildFromAmount(int ticketAmount)
    {
        return new Reward_Tickets(ticketAmount);
    }

    public override string ToString()
    {
        return amount + " tickets";
    }
}
