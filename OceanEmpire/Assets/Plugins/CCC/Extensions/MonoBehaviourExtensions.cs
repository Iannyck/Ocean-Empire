using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static Coroutine CallNextFrame(this MonoBehaviour source, Action action)
    {
        return source.StartCoroutine(DelayedCallTo(action, 0.00001f, true));
    }
    public static Coroutine DelayedCall(this MonoBehaviour source, Action action, float delay, bool realTime = false)
    {
        return source.StartCoroutine(DelayedCallTo(action, delay, realTime));
    }
    private static IEnumerator DelayedCallTo(Action action, float delay, bool realTime)
    {
        if (realTime) yield return new WaitForSecondsRealtime(delay);
        else yield return new WaitForSeconds(delay);

        action.Invoke();
    }
}
