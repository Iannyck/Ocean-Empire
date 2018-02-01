using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RefillCost
{
    private const int TicketCostForFullRefill = 20;

    public static CurrencyAmount GetRefillCost()
    {
        int cost = ( (1 - FishPopulation.PopulationRate) * TicketCostForFullRefill).RoundedToInt();
        return new CurrencyAmount(cost, CurrencyType.Ticket);
    }
}
