using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class DelayedAction : MonoBehaviour
    {

        public void Do(float delay, Action action)
        {
            if (action != null)
                StartCoroutine(Routine(delay, action));
        }

        IEnumerator Routine(float delay, Action action)
        {
            yield return new WaitForSecondsRealtime(delay);
            action();
        }
    }
}
