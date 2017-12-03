using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RefillCost
{
    private const int TicketCostForFullRefill = 20;

    public static Montant GetRefillCost()
    {
        int cost = ( (1 - FishPopulation.PopulationRate) * TicketCostForFullRefill).RoundedToInt();
        return new Montant(cost, CurrencyType.Ticket);
    }
}
