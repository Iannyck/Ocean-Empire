using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    public abstract class BasePrebuiltQuest<Q, C> : ScriptableObject, IQuestBuilder where C : QuestContext where Q : Quest<C>, new()
    {
        [SerializeField] protected C questContext;
        //[Suffix("hours"), SerializeField] protected float duration = -1;

        public Quest BuildQuest(DateTime questStartTime)
        {
            //var questDuration = new TimeSpan(0, Mathf.RoundToInt(duration * 60), 0);
            //var questTimeSlot = new TimeSlot(questStartTime, questDuration);

            var newQuest = new Q()
            {
                context = CCC.Utility.ObjectCopier.Clone(questContext),
                createdOn = questStartTime
            };

            OnBuildNewQuest(ref newQuest);
            return newQuest;
        }

        protected virtual void OnBuildNewQuest(ref Q newQuest) { }
    }
}
