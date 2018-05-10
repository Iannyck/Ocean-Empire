using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Market
{
    // NB: Pour l'instant, les valeurs sont hardcodé, mais elles devraient éventuellement varier en fonction de plusieurs paramètres.

    public static CurrencyAmount GetCurrencyAmountFromValue(CurrencyType type, MarketValue desiredValue, out MarketValue excessValue)
    {
        float floatingCoins = (desiredValue / GetCurrencyValue(type)).floatValue;
        int roundedCoins = Mathf.FloorToInt(floatingCoins);
        excessValue = (floatingCoins - roundedCoins) * GetCurrencyValue(type);

        return new CurrencyAmount(roundedCoins, type);
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
                return 0.33f;
            case ExerciseType.Run:
                return 0.33f;
            case ExerciseType.Stairs:
                return 0.33f;
            case ExerciseType.PressKey:
                return 0.33f;
            default:
                return -1;
        }
    }

    public static MarketValue GetExerciseValue(ExerciseVolume exerciseVolume)
    {
        return GetExerciseValue(exerciseVolume.type) * exerciseVolume.volume;
    }
}
