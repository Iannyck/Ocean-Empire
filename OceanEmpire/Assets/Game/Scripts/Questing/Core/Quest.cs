using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    public enum DirtyState
    {
        Clean = 0,

        /// <summary>
        /// Will be saved in the following seconds (this helps batch the regular save calls)
        /// </summary>
        Dirty,

        /// <summary>
        /// Will be immediately saved
        /// </summary>
        UrgentDirty
    }

    [Serializable]
    public abstract class Quest
    {
        //public TimeSlot timeSlot;
        public DateTime createdOn;
        public QuestState state = QuestState.NotStarted;

        //public QuestReward reward;

        [NonSerialized] public Action<Quest> onCompletion;
        [NonSerialized] public DirtyState DirtyState = DirtyState.Clean;

        public abstract QuestContext Context { get; }
        public abstract string GetDisplayedProgressText();
        public abstract float GetProgress01();
        public abstract void Launch();
        public abstract QuestState UpdateState();

        protected void Complete()
        {
            state = QuestState.Completed;
            if (onCompletion != null)
                onCompletion(this);
            DirtyState = DirtyState.UrgentDirty;
        }
    }

    [Serializable]
    public abstract class Quest<T> : Quest where T : QuestContext
    {
        public T context;
        public override QuestContext Context { get { return context; } }
    }
}
