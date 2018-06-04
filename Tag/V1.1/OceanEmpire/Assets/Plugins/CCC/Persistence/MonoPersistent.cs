using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Persistence
{
    public abstract class MonoPersistent : MonoBehaviour, IPersistent
    {
        public abstract void Init(Action onComplete);

        public UnityEngine.Object DuplicationBehavior()
        {
            var newObj = this.DuplicateGO();
            DontDestroyOnLoad(newObj);
            return newObj;
        }
    }
}