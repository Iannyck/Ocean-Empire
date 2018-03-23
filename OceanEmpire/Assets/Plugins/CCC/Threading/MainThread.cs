using UnityEngine;
using System.Collections.Generic;
using System;

namespace CCC.Threading
{
    public class MainThread : SelfSpawningSingleton<MainThread>
    {
        private static Queue<Action> actionList = new Queue<Action>();

        void Update()
        {
            while (actionList.Count > 0)
            {
                actionList.Dequeue()();
            }
        }

        static public void SpawnIfNotSpawned()
        {
            CheckInstance();
        }
        static public void AddActionFromThread(Action action)
        {
            if (action == null)
                return;

            CheckInstance();

            if (GetRawInstance() == null)
            {
                Debug.LogError("Aucune instance de MainThread");
                return;
            }
            lock (_Instance)
            {
                actionList.Enqueue(action);
            }
        }

        protected override string GameObjectName()
        {
            return "Main Thread";
        }
    }
}
