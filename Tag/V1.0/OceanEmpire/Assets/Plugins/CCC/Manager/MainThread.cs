using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace CCC.Manager
{
    public class MainThread : BaseManager<MainThread>
    {
        static List<Action> actionList = new List<Action>();
        public override void Init()
        {
            CompleteInit();
        }

        void Update()
        {
            if(actionList.Count > 0)
            {
                for(int i=0; i<actionList.Count; i++)
                {
                    actionList[i]();
                    actionList.RemoveAt(i);
                    i--;
                }
            }
        }

        static public void AddAction(Action action)
        {
            if (action == null) return;
            actionList.Add(action);
        }
    }
}
