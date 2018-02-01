using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RewardBuilder
{
    private const float nominalReward = 30f; // b dans r = ax +b
    const float constantGrowthPerLevel = 20f; // a dans r = ax +b

    const float levelDiffenceModifier = 0.20f; // pourcent gain or lost by level difference

    private static float RewardByLevel(int level)
    {
        float reward = level * constantGrowthPerLevel + nominalReward;
        return reward;
    }

    private static float RewardByRelativeLevel(int taskLevel, int playerLevel)
    {
        float levelModifier = 1 + levelDiffenceModifier * (taskLevel - playerLevel);
        float reward = RewardByLevel(taskLevel) * levelModifier;
        return reward;
    }

    private static float RewardByRelativeLevelAndType(int taskLevel, int playerLevel, RewardType type)
    {
        float baseValueMidifier = (float)(1 / RewardComponents.GetBaseValue(RewardType.Tickets));
        float reward = RewardByRelativeLevel(taskLevel, playerLevel) * baseValueMidifier;
        return reward;
    }

    public static int RevertRewardByLevel(float reward)
    {
        return ((reward * (float)RewardComponents.GetBaseValue(RewardType.Tickets) - nominalReward) / constantGrowthPerLevel).RoundedToInt().Clamped(0,taskDifficulty.MaxLevel);
    }

    public static Reward AutoReward(Task task, RewardType rewardType)
    {
        int taskLevel = taskDifficulty.GetTaskLevel(task);
        int playerLevel = PlayerProfile.Level;

        int rewardValue = RewardByRelativeLevelAndType(taskLevel, playerLevel, rewardType).RoundedToInt();

        switch (rewardType)
        {
            case RewardType.Coins:
                return Reward_Coins.BuildFromAmount(rewardValue);
            case RewardType.Tickets:
                return Reward_Tickets.BuildFromAmount(rewardValue);
            case RewardType.OceanRefill:
                return Reward_OceanRefill.Build(); 
        }
        return Reward_Tickets.BuildFromAmount(-1);
    }

    public static Reward BuildFromAmount(RewardType rewardType, int Amount)
    {
        switch (rewardType)
        {
            case RewardType.Coins:
                return Reward_Coins.BuildFromAmount(Amount);
            case RewardType.Tickets:
                return Reward_Tickets.BuildFromAmount(Amount);
            case RewardType.OceanRefill:
                return Reward_OceanRefill.Build(); 
        }
        return Reward_Tickets.BuildFromAmount(-1);
    }

    public static Reward BuildFromValue(RewardType rewardType, float value)
    {
        switch (rewardType)
        {
            case RewardType.Coins:
                return Reward_Coins.BuildFromValue(value);
            case RewardType.Tickets:
                return Reward_Tickets.BuildFromValue(value);
            case RewardType.OceanRefill:
                return Reward_OceanRefill.Build();
        }
        return Reward_Tickets.BuildFromValue(-1);
    }
}
