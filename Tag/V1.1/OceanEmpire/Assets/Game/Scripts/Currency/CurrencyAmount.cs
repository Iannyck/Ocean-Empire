using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CurrencyAmount
{
    public CurrencyType currencyType;
    public int amount;

    public CurrencyAmount(int amount, CurrencyType currencyType)
    {
        this.amount = amount;
        this.currencyType = currencyType;
    }

    public override string ToString()
    {
        return amount + " " + CurrencyComponents.GetDisplayName(currencyType);
    }

    public override bool Equals(object obj)
    {
        if (!(obj is CurrencyAmount))
        {
            return false;
        }

        var objM = (CurrencyAmount)obj;

        return objM.amount == amount && objM.currencyType == currencyType;
    }

    public override int GetHashCode()
    {
        return amount.GetHashCode();
    }

    public static bool operator ==(CurrencyAmount c1, CurrencyAmount c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(CurrencyAmount c1, CurrencyAmount c2)
    {
        return !c1.Equals(c2);
    }
}