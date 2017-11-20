using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward_Tickets : Reward
{
    public int amount;

    public override void Apply()
    {
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

    public Reward_Tickets BuildReverse(int value)
    {
        return new Reward_Tickets(Mathf.RoundToInt(value / RewardComponents.GetBaseValue(RewardType.Tickets)));
    }

    public Reward_Tickets Build(int ticketAmount)
    {
        return new Reward_Tickets(ticketAmount);
    }
}
