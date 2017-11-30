using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackingWindow : MonoBehaviour
{
    public const string SCENE_NAME = "TrackingWindow";

    // UI
    public Text currentTimeUI;
    public Slider completionState;
    public Text timeWaiting;

    // Animation
    public WindowAnimation windowAnim;

    // Tracking Initialisation
    private ExerciseTracker tracker;
    private DateTime trackingStart;
    private ScheduledTask currentTask;
    private ActivityAnalyser.Report currentReport;
    private List<GameObject> infoObjects = new List<GameObject>();

    public static void ShowWaitingWindow(string exerciceDescription, ScheduledTask task, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        Scenes.LoadAsync(SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            scene.FindRootObject<TrackingWindow>().InitDisplay(exerciceDescription, task, enAttente, title);
        }, true);
    }

    public static bool IsTrackingSomething()
    {
        return Scenes.Exists(SCENE_NAME);
    }

    public void InitDisplay(string exerciceDescription, ScheduledTask task, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        tracker = ExerciseComponents.GetTracker(task.task.GetExerciseType());
        trackingStart = DateTime.Now;
        currentTask = task;
    }

    public void UpdateInfo(int index, string info)
    {
        infoObjects[index].GetComponent<Text>().text = info;
    }

    private void Update()
    {
        currentReport = tracker.Track(currentTask);
        UpdateExerciceCompletion(currentReport.timeSpendingExercice, new TimeSpan(0, (int)((WalkTask)currentReport.task.task).minutesOfWalk, 0));
        if (currentReport.complete)
        {
            ConcludeTask(ExerciseTrackingReport.BuildFromNonInterrupted(currentReport));
            Hide();
        }
        if(DateTime.Now.CompareTo(currentTask.timeSlot.end) > 1)
            ForceStop();
    }

    public void Hide()
    {
        windowAnim.Close(delegate ()
        {
            Scenes.UnloadAsync(SCENE_NAME);
        });
    }

    private void ConcludeTask(ExerciseTrackingReport trackingReport)
    {
        TimedTaskReport taskReport = TimedTaskReport.BuildFromCompleted(currentTask, trackingReport, HappyRating.None);
        Calendar.instance.ConcludeScheduledTask(currentTask, taskReport);
    }

    public void ForceComplete()
    {
        currentReport = tracker.Track(currentTask);
        ConcludeTask(ExerciseTrackingReport.BuildFrom_UserSaidItWasCompleted(currentReport));
        Hide();
    }

    public void ForceStop()
    {
        currentReport = tracker.Track(currentTask);
        ConcludeTask(ExerciseTrackingReport.BuildFromAbandonned(currentReport));
        Hide();
    }

    private void UpdateExerciceCompletion(TimeSpan timeDone, TimeSpan timeToDo)
    {
        TimeSpan time = DateTime.Now.Subtract(trackingStart);
        timeWaiting.text = time.Hours + ":" + time.Minutes + ":" + time.Seconds;

        double totalTimeDone = timeDone.TotalSeconds; // secondes
        double totalTimeToDo = timeToDo.TotalSeconds; // secondes
        double completion = totalTimeDone / totalTimeToDo;

        if (completionState != null)
            completionState.value = (float)completion;

        currentTimeUI.text = "" + timeDone.Minutes.ToString() + ":" + timeDone.Seconds.ToString();
    }
}
