using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Market
{
    // NB: Pour l'instant, les valeurs sont hardcodé, mais elles devraient éventuellement varier en fonction de plusieurs paramètres.

    public static CurrencyAmount GetCurrencyAmountFromValue(CurrencyType type, MarketValue desiredValue)
    {
        int currencyAmount = (desiredValue / GetCurrencyValue(type)).floatValue.RoundedToInt();
        return new CurrencyAmount(currencyAmount, type);
    }

    public static MarketValue GetCurrencyValue(CurrencyType type)
    {
        switch (type)
        {
            default:
                return -1;
            case CurrencyType.Coin:
                return 1;
            case CurrencyType.Ticket:
                return 1;
        }
    }

    public static MarketValue GetOceanRefillValue()
    {
        return 15;
    }

    public static MarketValue GetExerciseValue(ExerciseType type)
    {
        switch (type)
        {
            case ExerciseType.Walk:
                return 1;
            case ExerciseType.Run:
                return 2;
            case ExerciseType.Stairs:
                return 2;
            case ExerciseType.PressKey:
                return 10;
            default:
                return -1;
        }
    }

    public static MarketValue GetExerciseValue(ExerciseVolume exerciseVolume)
    {
        return GetExerciseValue(exerciseVolume.type) * exerciseVolume.volume;
    }
}
