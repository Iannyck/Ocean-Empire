using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Montant
{
    public CurrencyType currencyType;
    public int amount;
    public Montant(int amount, CurrencyType currencyType)
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
        if (!(obj is Montant))
        {
            return false;
        }

        var objM = (Montant)obj;

        return objM.amount == amount && objM.currencyType == currencyType;
    }

    public override int GetHashCode()
    {
        return amount.GetHashCode();
    }

    public static bool operator ==(Montant c1, Montant c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(Montant c1, Montant c2)
    {
        return !c1.Equals(c2);
    }
}