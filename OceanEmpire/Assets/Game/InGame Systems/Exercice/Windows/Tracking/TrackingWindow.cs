using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TrackingWindow : MonoBehaviour
{
    public const string SCENE_NAME = "TrackingWindow";

    // UI
    public Text currentTimeUI;
    public Text confidenceDisplay;
    public Slider completionState;
    public Text timeWaiting;
    public float sliderAnimDuration = 1f;
    public float constantAugmentationRate = 10;
    public Text pourcentDisplay;

    // Animation
    public WindowAnimation windowAnim;

    // Tracking Initialisation
    private float currentPourcent;
    private float constantAugmentation;
    private float previousSliderValue;
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
        previousSliderValue = 0;
        currentPourcent = 0;
    }

    public void UpdateInfo(int index, string info)
    {
        infoObjects[index].GetComponent<Text>().text = info;
    }

    private void Update()
    {
        currentReport = tracker.Track(currentTask);

        constantAugmentation = ((int)((WalkTask)currentReport.task.task).minutesOfWalk) / (Mathf.Min(constantAugmentationRate,1) * 100000);

        ConstantAugmentation();

        DisplayCurrentConfidence();

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

        pourcentDisplay.text = ((float)completion * 100) + "%";

        if (completionState != null)
        {
            if (previousSliderValue < (float)completion)
            {
                completionState.DOValue((float)completion, sliderAnimDuration);
                previousSliderValue = (float)completion;
            }
            else
            {
                if ((completionState.value + constantAugmentation) >= 90)
                    completionState.value = 90; 
                completionState.value = completionState.value + constantAugmentation;
            }
        }

        currentTimeUI.text = "" + timeDone.Minutes.ToString() + ":" + timeDone.Seconds.ToString();
    }

    private void ConstantAugmentation()
    {
        constantAugmentation = ((int)((WalkTask)currentReport.task.task).minutesOfWalk) / (Mathf.Min(constantAugmentationRate, 1) * 100000);
    }

    private void DisplayCurrentConfidence()
    {
        if(ActivityAnalyser.instance.GetLast() != null)
            confidenceDisplay.text = ActivityAnalyser.instance.GetLast().probability.ToString();
    }
}
