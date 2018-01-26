using UnityEngine;
using System.Collections.Generic;
using System;
using CCC.Persistence;

namespace CCC.Threading
{
    public class MainThread : MonoPersistent
    {
        private static MainThread instance;
        private static Queue<Action> actionList = new Queue<Action>();

        void Awake()
        {
            instance = this;
        }

        public override void Init(Action onComplete)
        {
            onComplete();
        }

        void Update()
        {
            if (actionList.Count > 0)
            {
                while (actionList.Count > 1)
                {
                    actionList.Dequeue()();
                }
            }
        }

        static public void AddActionFromThread(Action action)
        {
            if (instance == null)
            {
                Debug.LogError("Aucune instance de MainThread");
                return;
            }
            lock (instance)
            {
                if (action == null) return;
                actionList.Enqueue(action);
            }
        }
    }
}
