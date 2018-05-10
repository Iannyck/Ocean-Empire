using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : MonoBehaviour
{
    public TaskPanel_Empty emptyState;
    public TaskPanel_ReadyToCreate readyToCreateState;
    public TaskPanel_Ongoing ongoingState;
    public TaskPanel_Planned plannedState;

    [Suffix("seconds")]
    public float updateStateEvery = 5;

    private ITaskPanelState currentState;
    private float updateTimer;

    void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            UpdateState();
            PlannedExerciceRewarder.Instance.OnLatestPendingReportUpdated += UpdateState;

            if (MapManager.Instance.MapIndex < 1)
            {
                gameObject.SetActive(false);
            }
        });
    }

    void OnDestroy()
    {
        if (PlannedExerciceRewarder.Instance != null)
            PlannedExerciceRewarder.Instance.OnLatestPendingReportUpdated -= UpdateState;
    }

    //void Update()
    //{
    //    if (updateTimer <= 0)
    //    {
    //        UpdateState();
    //        updateTimer += updateStateEvery;
    //    }

    //    updateTimer -= Time.deltaTime;
    //}

    void UpdateState()
    {
        if (PlannedExerciceRewarder.Instance == null)
        {
            Debug.LogError("Need PlannedExerciceRewarder!!");
            return;
        }

        var latestReport = PlannedExerciceRewarder.Instance.LatestPendingReport;

        if (latestReport != null)
        {
            switch (latestReport.state)
            {
                case PlannedExerciceRewarder.Report.State.Ongoing:
                    // Ongoing exercise
                    ongoingState.FillContent(latestReport);
                    TransitionToState(ongoingState);
                    break;
                case PlannedExerciceRewarder.Report.State.Completed:
                case PlannedExerciceRewarder.Report.State.Failed:
                    // We're waiting for the conclusion of a past exercise (ex: reward window)
                    TransitionToState(emptyState);
                    break;
            }
        }
        else
        {
            // Check for future plannings
            var presentAndFuture = Calendar.instance.GetPresentAndFutureSchedules();
            if (presentAndFuture.Count == 0)
            {
                TransitionToState(readyToCreateState);
            }
            else
            {
                var closestTask = presentAndFuture[0];
                if (closestTask.timeSlot.IsNow())
                {
                    // Now ! (we should wait for an official PlannedExerciceRewarder.Report before going in OnGoing state)
                    TransitionToState(emptyState);
                }
                else
                {
                    // In the future
                    plannedState.FillContent(closestTask);
                    TransitionToState(plannedState);
                }
            }
        }
    }

    void TransitionToState(ITaskPanelState taskPanelState, Action onComplete = null, bool forceTransition = false)
    {
        if (taskPanelState == currentState && !forceTransition)
            return;

        if (currentState == null)
        {
            currentState = taskPanelState;
            taskPanelState.Enter(onComplete);
        }
        else
        {
            currentState.Exit(() =>
            {
                currentState = taskPanelState;
                taskPanelState.Enter(onComplete);
            });
        }
    }
}


public interface ITaskPanelState
{
    void Exit(Action onComplete);
    void Enter(Action onComplete);
    void FillContent(object data);
}