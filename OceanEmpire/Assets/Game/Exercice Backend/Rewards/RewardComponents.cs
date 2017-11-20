using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RewardComponents
{
    public static float GetBaseValue(RewardType type)
    {
        switch (type)
        {
            case RewardType.Coins:
                return 1;
            case RewardType.Tickets:
                return 10;
            case RewardType.OceanRefill:
                return 30;
        }
        return -1;
    }
    public static string GetDisplayName(RewardType type)
    {
        switch (type)
        {
            case RewardType.Coins:
                return "Jeton";
            case RewardType.Tickets:
                return "Tickets";
            case RewardType.OceanRefill:
                return "Remplissage de poisson";
        }
        return "";
    }
    public static string GetDescription(RewardType type)
    {
        switch (type)
        {
            case RewardType.Coins:
                return "Dé jeton, cé dé jeton!";
            case RewardType.Tickets:
                return "Un ticket, c qqchose de noice.";
            case RewardType.OceanRefill:
                return "PLUS DE POISSON ! PLUUUS!!!";
        }
        return "";
    }

    public static List<RewardType> GetAllTypes()
    {
        return new List<RewardType>()
        {
            RewardType.Coins,
            RewardType.Tickets,
            RewardType.OceanRefill
        };
    }
}
