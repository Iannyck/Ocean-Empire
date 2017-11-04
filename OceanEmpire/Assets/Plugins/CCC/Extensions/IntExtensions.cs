using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntExtensions
{

    public static int Abs(this int value)
    {
        return Mathf.Abs(value);
    }

    /// <summary>
    /// Reduis potentiellement la valeur
    /// </summary>
    public static int Capped(this int value, int max)
    {
        return Mathf.Min(value, max);
    }

    /// <summary>
    /// Augmente potentiellement la valeur
    /// </summary>
    public static int Raised(this int value, int min)
    {
        return Mathf.Max(value, min);
    }

    public static float Powed(this int value, float exponent)
    {
        return Mathf.Pow(value, exponent);
    }

    public static int Sign(this int value)
    {
        return value > 0 ? 1 : -1;
    }

    public static int Clamped(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static int Mod(this int value, int modulo)
    {
        if (modulo < 1)
            return 0;
        
        if (value < 0)
        {
            value += Mathf.CeilToInt((float)value.Abs() / modulo) * modulo;
        }

        return value % modulo;
    }
}
