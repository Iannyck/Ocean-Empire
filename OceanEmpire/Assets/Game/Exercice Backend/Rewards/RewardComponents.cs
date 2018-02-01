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
                return CurrencyValues.GetAbsoluteValue(CurrencyType.Coin);
            case RewardType.Tickets:
                return CurrencyValues.GetAbsoluteValue(CurrencyType.Ticket);
            case RewardType.OceanRefill:
                return CurrencyValues.GetAbsoluteValue(CurrencyType.Ticket) * 1.5f;
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
