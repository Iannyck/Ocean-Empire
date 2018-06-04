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

    /// <summary>
    /// Retourne le 'power of two' le plus pres, en descendant. Ex: 0010110 -> 0010000
    /// </summary>
    public static int GetLowerPowerOfTwo(this int value)
    {
        if (value < 0)
            value = -value;
        
        int newVal = value;
        while (newVal != 0)
        {
            value = newVal;
            newVal &= (value - 1);
        }
        return value;
    }

    /// <summary>
    /// Retourne le numero du bit a 1 le plus a gauche. Ex: 0010110 -> 4
    /// </summary>
    public static int GetLeftmostSetBit(this int value)
    {
        if (value < 0)
            value = -value;

        int i = 0;
        while (value != 1)
        {
            value >>= 1;
            i++;
        }
        return i;
    }

    public static bool IsEvenNumber(this int value)
    {
        return (value & 1) == 0;
    }
}
