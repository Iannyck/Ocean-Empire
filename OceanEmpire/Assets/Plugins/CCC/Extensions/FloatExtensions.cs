using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    public static Vector2 ToVector(this float value)
    {
        return CCC.Math.Vectors.AngleToVector(value);
    }

    public static float Abs(this float value)
    {
        return Mathf.Abs(value);
    }

    public static float Rounded(this float value)
    {
        return Mathf.Round(value);
    }

    public static float RoundedToInt(this float value)
    {
        return Mathf.RoundToInt(value);
    }

    /// <summary>
    /// Reduis potentiellement la valeur
    /// </summary>
    public static float Capped(this float value, float max)
    {
        return Mathf.Min(value, max);
    }
    
    public static float Sign(this float value)
    {
        return Mathf.Sign(value);
    }

    /// <summary>
    /// Augmente potentiellement la valeur
    /// </summary>
    public static float Floored(this float value, float min)
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
            value = 0;
        else
            return value % modulo;
        return value;
    }
}
