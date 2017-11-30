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
    public Text lastProb;
    public Text timeSinceStart;

    // Animation
    public WindowAnimation windowAnim;

    // Tracking Initialisation
    private DateTime startTime;
    private ExerciseTracker tracker;
    private TimeSpan currentRemaining;
    private bool startTrackingUpdate;
    private DateTime trackingStart;
    private ScheduledTask currentTask;
    private ActivityAnalyser.Report currentReport;
    private List<GameObject> infoObjects = new List<GameObject>();

    public static void ShowWaitingWindow(string exerciceDescription, ScheduledTask task, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        Scenes.LoadAsync(SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            scene.FindRootObject<TrackingWindow>().InitDisplay(exerciceDescription, task, enAttente, title);
        });
    }

    public static bool IsTrackingSomething()
    {
        return Scenes.Exists(SCENE_NAME);
    }

    private void Awake()
    {
        startTrackingUpdate = false;
        currentRemaining = new TimeSpan(0, 0, 0);

    }

    public void InitDisplay(string exerciceDescription, ScheduledTask task, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        tracker = ExerciseComponents.GetTracker(task.task.GetExerciseType());
        trackingStart = DateTime.Now;
        currentTask = task;
        startTrackingUpdate = true;
        startTime = DateTime.Now;
    }

    public void UpdateInfo(int index, string info)
    {
        infoObjects[index].GetComponent<Text>().text = info;
    }

    private void Update()
    {
        if (startTrackingUpdate)
        {
            currentReport = tracker.UpdateTracking(currentTask, trackingStart); // task=TimedTask, startedWhen=DateTime
            UpdateExerciceCompletion(currentReport.timeSpendingExercice, new TimeSpan(0, (int)((WalkTask)currentReport.task.task).minutesOfWalk, 0));
            if (currentReport.complete)
            {
                ConcludeTask(ExerciseTrackingReport.BuildFromNonInterrupted(currentReport));
                Hide(); // exercise complete ! state=ExerciseTrackingReport.State 
            }
        }
    }

    public void Hide()
    {
        startTrackingUpdate = false;
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
        currentReport = tracker.EvaluateTask(currentTask);
        ConcludeTask(ExerciseTrackingReport.BuildFrom_UserSaidItWasCompleted(currentReport));
        Hide();
    }

    public void ForceStop()
    {
        currentReport = tracker.EvaluateTask(currentTask);
        ConcludeTask(ExerciseTrackingReport.BuildFromAbandonned(currentReport));
        Hide();
    }

    private void UpdateExerciceCompletion(TimeSpan timeDone, TimeSpan timeToDo)
    {
        if (currentReport.probabilities.Count > 0)
            lastProb.text = currentReport.probabilities.Last().ToString() + "%";
        else
            lastProb.text = "NO PROB %";

        timeSinceStart.text = DateTime.Now.Subtract(startTime).ToString();

        double totalTimeDone = timeDone.TotalSeconds; // secondes
        double totalTimeToDo = timeToDo.TotalSeconds; // secondes
        double completion = totalTimeDone / totalTimeToDo;

        //Debug.Log("TIME DONE :" + totalTimeDone);
        //Debug.Log("TIME TO DO :" + totalTimeToDo);
        //Debug.Log("COMPLETION :" + (completion * 100) + "%");

        if (completionState != null)
            completionState.value = (float)completion;

        currentTimeUI.text = "" + timeDone.Minutes.ToString() + ":" + timeDone.Seconds.ToString();
    }
}
