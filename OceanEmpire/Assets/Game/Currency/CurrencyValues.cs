using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrencyValues
{
    public static float GetAbsoluteValue(CurrencyType type)
    {
        // NB: Pour l'instant, la valeur est hardcodé, mais elle devrait éventuellement varié en fonction de plusieurs paramètre.

        switch (type)
        {
            default:
            case CurrencyType.Coin:
                return 1;
            case CurrencyType.Ticket:
                return 10;
        }
    }

    /// <summary>
    /// La valeur relative entre deux currency (a / b). Ex: 10 -> a vaut 10x plus que b
    /// </summary>
    public static float GetRelativeValue(CurrencyType a, CurrencyType b)
    {
        return GetAbsoluteValue(a) / GetAbsoluteValue(b);
    }
}
