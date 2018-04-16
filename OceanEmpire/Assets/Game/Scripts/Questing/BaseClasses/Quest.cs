using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public abstract class Quest
    {
        public TimeSlot timeSlot;
        public QuestState state = QuestState.NotStarted;

        public abstract QuestContext Context { get; }
        public abstract void StartTracking();
        public abstract QuestState UpdateState();
    }
    [Serializable]
    public abstract class Quest<T> : Quest where T : QuestContext
    {
        public T context;
        public override QuestContext Context { get { return context; } }
    }
}
