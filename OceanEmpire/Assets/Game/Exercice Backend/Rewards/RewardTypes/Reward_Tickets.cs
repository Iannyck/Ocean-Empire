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

<<<<<<< .merge_file_a16912
    public Reward_Tickets BuildReverse(int value)
=======
    public static Reward_Tickets BuildReverse(int value)
>>>>>>> .merge_file_a11320
    {
        return new Reward_Tickets(Mathf.RoundToInt(value / RewardComponents.GetBaseValue(RewardType.Tickets)));
    }

<<<<<<< .merge_file_a16912
    public Reward_Tickets Build(int ticketAmount)
=======
    public static Reward_Tickets Build(int ticketAmount)
>>>>>>> .merge_file_a11320
    {
        return new Reward_Tickets(ticketAmount);
    }
}
