using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fixes your lerp (based on 120 fps)
/// </summary>
public static class FixedLerp
{
    public const float defaultFPS = 120;
    private static float FPS
    {
        get { return 1f / Time.deltaTime; }
    }
    //private static float UnscaledFPS
    //{
    //    get { return 1f / Time.unscaledDeltaTime; }
    //}
    private static float FixedFPS
    {
        get { return 1f / Time.fixedDeltaTime; }
    }
    //private static float FixedUnscaledFPS
    //{
    //    get { return 1f / Time.fixedUnscaledTime; }
    //}

    public static float Fix(float lerpAmount)
    {
        return Fix(lerpAmount, FPS);
    }

    public static float FixedFix(float lerpAmount)
    {
        return Fix(lerpAmount, FixedFPS);
    }

    public static float FixScaled(float lerpAmount, float timeScale)
    {
        return Fix(lerpAmount, FPS*timeScale);
    }

    public static float FixedFixScaled(float lerpAmount, float timeScale)
    {
        return Fix(lerpAmount, FixedFPS * timeScale);
    }

    public static float Fix(float lerpAmount, float customFPS)
    {
        return 1 - Mathf.Pow(1 - Mathf.Min(lerpAmount,1), defaultFPS / customFPS);
    }

    //public static float UnscaledFix(float lerpAmount)
    //{
    //    return Fix(lerpAmount, UnscaledFPS);
    //}

    //public static float FixedUnscaledFix(float lerpAmount)
    //{
    //    return Fix(lerpAmount, FixedUnscaledFPS);
    //}
}
