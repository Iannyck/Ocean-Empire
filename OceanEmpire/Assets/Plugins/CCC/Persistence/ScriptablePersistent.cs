using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Persistence
{
    public abstract class ScriptablePersistent : ScriptableObject, IPersistent
    {
        public abstract void Init(Action onComplete);

        public UnityEngine.Object DuplicationBehavior()
        {
            return this;
        }
    }
}
