using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Questing
{
    [Serializable]
    public class PlanExercise : Quest<PlanExerciseContext>
    {
        int plannedYet;

        [NonSerialized] bool isListening = false;

        public override string GetDisplayedProgressText()
        {
            return plannedYet + " / " + context.planCount;
        }

        public override float GetProgress01()
        {
            return plannedYet / (float)context.planCount;
        }

        public override void Launch()
        {
            state = QuestState.Ongoing;
        }

        public override QuestState UpdateState()
        {
            if (state == QuestState.Ongoing)
                ListenIfNotListening();
            return state;
        }

        void ListenIfNotListening()
        {
            if (isListening)
                return;

            Calendar.instance.OnScheduleAdded += Calendar_OnScheduleAdded;
            isListening = true;
        }

        void StopListening()
        {
            Calendar.instance.OnScheduleAdded -= Calendar_OnScheduleAdded;
            isListening = false;
        }

        private void Calendar_OnScheduleAdded()
        {
            plannedYet++;

            if (plannedYet >= context.planCount)
            {
                Complete();
                StopListening();
            }
            else
            {
                DirtyState = DirtyState.UrgentDirty;
            }
        }
    }
}