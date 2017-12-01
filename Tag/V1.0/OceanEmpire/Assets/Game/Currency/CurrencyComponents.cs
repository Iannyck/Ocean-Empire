using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyComponents
{
    public static string GetDisplayName(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Coin:
                return "jeton";
            case CurrencyType.Ticket:
                return "ticket";
        }
        return "Non-supported type";
    }
}
