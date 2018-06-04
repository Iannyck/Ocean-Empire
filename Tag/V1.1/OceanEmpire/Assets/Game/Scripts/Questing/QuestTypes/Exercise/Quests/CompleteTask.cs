using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Questing
{
    [Serializable]
    public class CompleteTask : Quest<CompleteTaskContext>
    {
        [SerializeField] int completedSoFar = 0;

        [NonSerialized] bool isListening = false;

        public override string GetDisplayedProgressText()
        {
            return completedSoFar + "/" + context.completeCount;
        }

        public override float GetProgress01()
        {
            return completedSoFar / (float)context.completeCount;
        }

        public override void Launch()
        {
            state = QuestState.Ongoing;
            DirtyState = DirtyState.Dirty;
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

            PlannedExerciceRewarder.Instance.OnReportFinalized += OnReportFinalized;
            isListening = true;
        }

        void StopListening()
        {
            PlannedExerciceRewarder.Instance.OnReportFinalized -= OnReportFinalized;
            isListening = false;
        }

        private void OnReportFinalized(PlannedExerciceRewarder.Report report)
        {
            switch (report.state)
            {
                case PlannedExerciceRewarder.Report.State.Completed:
                    completedSoFar++;
                    break;
                case PlannedExerciceRewarder.Report.State.Failed:
                    if (context.inARow)
                        completedSoFar = 0;
                    break;
                default:
                    break;
            }

            if (completedSoFar >= context.completeCount)
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