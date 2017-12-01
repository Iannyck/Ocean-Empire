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
    public Text currentExerciseDoneTime;
    public Text confidenceDisplay;
    public Slider completionState;
    public Text timeWaiting;
    public float sliderAnimDuration = 1f;
    public Text pourcentDisplay;

    // Animation
    public WindowAnimation windowAnim;

    // Tracking Initialisation
    private bool inSliderAnim;
    private float constantAugmentation;
    private float currentPourcent;
    private float previousSliderValue;
    private ExerciseTracker tracker;
    private DateTime trackingStart;
    private ScheduledTask currentTask;
    private ActivityAnalyser.Report currentReport;
    private bool trackingOver;
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
        inSliderAnim = false;
        trackingOver = false;
        // Augmentation du slider
        constantAugmentation = (((int)((WalkTask)currentTask.task).minutesOfWalk) * 0.00005f) / 3;
    }

    public void UpdateInfo(int index, string info)
    {
        infoObjects[index].GetComponent<Text>().text = info;
    }

    private void Update()
    {
        currentReport = tracker.Track(currentTask);

        DisplayCurrentConfidence();

        UpdateUI(currentReport.timeSpendingExercice, ((WalkTask)currentReport.task.task).timeOfWalk);
        if (currentReport.complete)
        {
            if (!trackingOver)
            {
                Conclude_WithCompletion();
            }
        }
        else if (currentTask.timeSlot.IsInThePast())
        {
            // Si l'exercice est trop long
            if (!trackingOver)
                Conclude_WithCompletion();
        }
    }

    public void Hide()
    {
        windowAnim.Close(delegate ()
        {
            Scenes.UnloadAsync(SCENE_NAME);
        });
    }

    public void Conclude_WithCompletion()
    {
        currentReport = tracker.Track(currentTask);
        ExerciseTrackingReport trackingReport = ExerciseTrackingReport.BuildFromNonInterrupted(currentReport);
        TimedTaskReport timedTaskReport = TimedTaskReport.BuildFromCompleted(currentTask, trackingReport, HappyRating.None);
        Conclude_AndCloseWindow(timedTaskReport);
    }

    public void Conclude_WithAbandon()
    {
        currentReport = tracker.Track(currentTask);
        ExerciseTrackingReport trackingReport = ExerciseTrackingReport.BuildFromAbandonned(currentReport);
        TimedTaskReport timedTaskReport = TimedTaskReport.BuildFromInterrupted(currentTask, trackingReport);
        Conclude_AndCloseWindow(timedTaskReport);
    }

    public void Conclude_WithUserSaidItsComplete()
    {
        currentReport = tracker.Track(currentTask);
        ExerciseTrackingReport trackingReport = ExerciseTrackingReport.BuildFrom_UserSaidItWasCompleted(currentReport);
        TimedTaskReport timedTaskReport = TimedTaskReport.BuildFromInterrupted(currentTask, trackingReport);
        Conclude_AndCloseWindow(timedTaskReport);
    }

    private void Conclude_AndCloseWindow(TimedTaskReport taskReport)
    {
        if (trackingOver)
            return;

        trackingOver = true;
        Hide();
        Calendar.instance.ConcludeScheduledTask(currentTask, taskReport);
    }

    private void UpdateUI(TimeSpan timeDone, TimeSpan timeToDo)
    {
        TimeSpan timeSinceStart = DateTime.Now - currentReport.task.timeSlot.start;
        timeWaiting.text = "Temps écoulé: " + timeSinceStart.Hours + ":" + timeSinceStart.Minutes + ":" + timeSinceStart.Seconds;

        currentExerciseDoneTime.text = "Exer. done: " + timeDone.Minutes.ToString() + ":" + timeDone.Seconds.ToString();

        double totalTimeDone = timeDone.TotalSeconds; // secondes
        double totalTimeToDo = timeToDo.TotalSeconds; // secondes
        double completion = totalTimeDone / totalTimeToDo;

        UpdateSlider((float)completion);

        pourcentDisplay.text = ((float)completion * 100).Rounded(0) + "%";
    }

    private void DisplayCurrentConfidence()
    {
        if (ActivityAnalyser.instance.GetLast() != null)
            confidenceDisplay.text = ActivityAnalyser.instance.GetLast().probability.ToString();
    }

    private void UpdateSlider(float completion)
    {
        if (inSliderAnim)
            return;

        // Update du Slider
        if (completionState != null)
        {

            // Si on a terminé
            if (completion >= 1)
            {   // Slider doit être complet
                completionState.value = 1;
                return;
            }

            // Si on a complété plus que ce que le slider montre
            if (completion > completionState.value)
            {
                // On fait une anim pour modifié le slider
                inSliderAnim = true;
                completionState.DOValue(completion, sliderAnimDuration).OnComplete(delegate () { inSliderAnim = false; });
                // On enregistre la dernière fois qu'on a capté l'exercice et update le slider
                previousSliderValue = completion;
            }
            else
            {
                // On augmente constamment la monté du slider jusqu'à 10% plus élevé que la derniere fois qu'un exercice a été capté
                if ((completionState.value + constantAugmentation) > (previousSliderValue + 0.1f))
                {
                    if ((previousSliderValue + 0.1f) >= 1)
                        completionState.value = 1;
                    else
                        completionState.value = (previousSliderValue + 0.1f);
                }
                else
                {
                    if (completionState.value + constantAugmentation >= 1)
                        completionState.value = 1;
                    else
                        completionState.value = completionState.value + constantAugmentation;
                }
            }
        }
    }
}
