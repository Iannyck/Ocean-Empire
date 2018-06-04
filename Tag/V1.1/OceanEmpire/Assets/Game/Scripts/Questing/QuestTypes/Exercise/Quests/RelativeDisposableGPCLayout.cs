using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

namespace Questing
{
    [Serializable]
    public class RelativeDisposableGPCLayout : Quest<RelativeDisposableGPCLayoutContext>
    {
        [SerializeField] int completedSoFar = 0;

        [NonSerialized] bool isListening = false;
        [NonSerialized] bool hasAwaken = false;

        public override string GetDisplayedProgressText()
        {
            return completedSoFar + "/" + context.amountRequired;
        }

        public override float GetProgress01()
        {
            return completedSoFar / (float)context.amountRequired;
        }

        public override void Launch()
        {
            state = QuestState.Ongoing;
        }

        void ProcessAwake()
        {
            if (hasAwaken)
                return;
            hasAwaken = true;
            PersistentLoader.LoadIfNotLoaded(() =>
            {
                if (state != QuestState.Completed)
                {
                    if (context.countInADay)
                    {
                        CountInDayCheck(DateTime.Now);
                    }
                    else
                    {
                        DayStreakCheck(DateTime.Now);
                    }
                }
            });
        }

        public override QuestState UpdateState()
        {
            if (state == QuestState.Ongoing)
            {
                ProcessAwake();
                ListenIfNotListening();
            }
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
            if (context.countInADay)
            {
                CountInDayCheck(report.schedule.timeSlot.end);
            }
            else
            {
                DayStreakCheck(report.schedule.timeSlot.end);
            }
        }


        #region Day Streak
        bool HasAtLeastOneInThatDay(DateTime date, ReadOnlyCollection<PlannedExerciceRewarder.Report> reports)
        {
            foreach (var r in reports)
            {
                if (r.state == PlannedExerciceRewarder.Report.State.Completed
                    && r.schedule.timeSlot.end.Day == date.Day)
                    return true;
            }
            return false;
        }
        void DayStreakCheck(DateTime date)
        {
            completedSoFar = 0;
            var reports = PlannedExerciceRewarder.Instance.GetPreviousReports();
            for (int i = 0; i < context.amountRequired; i++)
            {
                if (HasAtLeastOneInThatDay(date.AddDays(-i), reports))
                    completedSoFar++;

                // Si on a checké la journée précédente et on a rien, on abandonne là
                if (completedSoFar == 0 && i > 0)
                    break;
            }

            if (completedSoFar >= context.amountRequired)
            {
                Complete();
                StopListening();
            }
            else
            {
                DirtyState = DirtyState.UrgentDirty;
            }
        }
        #endregion

        #region Count in day
        int GetCompletedOnThisDay(DateTime date, ReadOnlyCollection<PlannedExerciceRewarder.Report> reports)
        {
            var total = 0;

            foreach (var r in reports)
            {
                if (r.state == PlannedExerciceRewarder.Report.State.Completed
                    && r.schedule.timeSlot.end.Day == date.Day)
                    total++;
            }
            return total;
        }

        void CountInDayCheck(DateTime date)
        {
            completedSoFar = GetCompletedOnThisDay(date, PlannedExerciceRewarder.Instance.GetPreviousReports());

            if (completedSoFar >= context.amountRequired)
            {
                Complete();
                StopListening();
            }
            else
            {
                DirtyState = DirtyState.UrgentDirty;
            }
        }
        #endregion
    }
}