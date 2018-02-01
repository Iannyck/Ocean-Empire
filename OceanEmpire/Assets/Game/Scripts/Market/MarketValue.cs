using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MarketValue
{
    public float floatValue;

    public MarketValue(float floatValue)
    {
        this.floatValue = floatValue;
    }

    #region Standard struc overrides
    public override string ToString()
    {
        return floatValue.ToString();
    }

    public override bool Equals(object obj)
    {
        return floatValue.Equals(obj);
    }

    public override int GetHashCode()
    {
        return floatValue.GetHashCode();
    }

    #endregion

    #region Operators
    public static MarketValue operator +(MarketValue a, MarketValue b)
    {
        return a.floatValue + b.floatValue;
    }
    public static MarketValue operator *(MarketValue a, MarketValue b)
    {
        return a.floatValue * b.floatValue;
    }
    public static MarketValue operator /(MarketValue a, MarketValue b)
    {
        return a.floatValue / b.floatValue;
    }
    public static MarketValue operator ++(MarketValue a)
    {
        return a.floatValue + 1;
    }
    public static MarketValue operator %(MarketValue a, float modulo)
    {
        return a.floatValue % modulo;
    }
    public static MarketValue operator %(MarketValue a, int modulo)
    {
        return a.floatValue % modulo;
    }
    public static bool operator ==(MarketValue a, MarketValue b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(MarketValue a, MarketValue b)
    {
        return !a.Equals(b);
    }
    public static explicit operator float(MarketValue val)
    {
        return val.floatValue;
    }
    public static implicit operator MarketValue(float val)
    {
        return new MarketValue(val);
    }
    #endregion
}
