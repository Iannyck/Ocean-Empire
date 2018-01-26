using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    /// <summary>
    /// Retourne le vecteur unitaire avec l'angle spécifié
    /// </summary>
    public static Vector2 ToVector(this float value)
    {
        float rad = value * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    public static float Abs(this float value)
    {
        return Mathf.Abs(value);
    }

    public static float Rounded(this float value)
    {
        return Mathf.Round(value);
    }

    public static float RoundedTo(this float value, float stepSize)
    {
        return (value/stepSize).Rounded() * stepSize;
    }

    public static float Powed(this float value, float exponent)
    {
        return Mathf.Pow(value, exponent);
    }

    public static float Rounded(this float value, int numberOfDecimal)
    {
        float mult = Mathf.Pow(10, numberOfDecimal);
        return (value * mult).Rounded() / mult;
    }

    public static int RoundedToInt(this float value)
    {
        return Mathf.RoundToInt(value);
    }

    public static float Sign(this float value)
    {
        return Mathf.Sign(value);
    }

    /// <summary>
    /// Reduis potentiellement la valeur
    /// </summary>
    public static float Capped(this float value, float max)
    {
        return Mathf.Min(value, max);
    }

    /// <summary>
    /// Augmente potentiellement la valeur
    /// </summary>
    public static float Raised(this float value, float min)
    {
        return Mathf.Max(value, min);
    }

    public static float Clamped(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }

    public static float MovedTowards(this float value, float target, float delta)
    {
        return Mathf.MoveTowards(value, target, delta);
    }

    public static float Lerpped(this float value, float target, float t)
    {
        return Mathf.Lerp(value, target, t);
    }

    public static float Mod(this float value, float modulo)
    {
        if (modulo <= 0)
            return 0;

        if (value < 0)
        {
            value += Mathf.Ceil(value.Abs() / modulo) * modulo;
        }

        return value % modulo;
    }
}
