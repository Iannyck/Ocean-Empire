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

    public override float GetValue()
    {
        return amount * RewardComponents.GetBaseValue(GetRewardType());
    }

    private Reward_Tickets(int tickets)
    {
        amount = tickets;
    }

    public override RewardType GetRewardType()
    {
        return RewardType.Tickets;
    }

    public static Reward_Tickets BuildReverse(int value)
    {
        return new Reward_Tickets(Mathf.RoundToInt(value / RewardComponents.GetBaseValue(RewardType.Tickets)));
    }

    public static Reward_Tickets Build(int ticketAmount)
    {
        return new Reward_Tickets(ticketAmount);
    }

    public override string ToString()
    {
        return amount + " tickets";
    }
}
