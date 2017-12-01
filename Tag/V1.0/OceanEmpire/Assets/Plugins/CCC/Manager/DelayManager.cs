using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace CCC.Manager
{
    public class DelayManager : BaseManager<DelayManager>
    {
        static public Coroutine LocalCallTo(Action action, float delay, MonoBehaviour source, bool realTime = false)
        {
            return source.StartCoroutine(DelayedCallTo(action, delay, realTime));
        }

        static public Coroutine CallNextFrame(Action action)
        {
            if (instance == null)
            {
                Debug.LogError("Tried to call a delay, but the manager is null. Was it properly loaded by MasterManager ?");
                return null;
            }
            return instance.StartCoroutine(DelayedCallTo(action, 0, true));
        }

        public override void Init()
        {
            CompleteInit();
        }

        static IEnumerator DelayedCallTo(Action action, float delay, bool realTime)
        {
            if (realTime) yield return new WaitForSecondsRealtime(delay);
            else yield return new WaitForSeconds(delay);
            
            action.Invoke();
        }
    }
}
