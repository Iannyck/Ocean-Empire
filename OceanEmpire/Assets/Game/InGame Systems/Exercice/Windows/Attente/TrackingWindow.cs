using CCC.Manager;
using CCC.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrackingWindow : MonoBehaviour {

    public const string SCENE_NAME = "TrackingWindow";

    public Text title;
    public Text exercice;
    public Text enAttente;
    public Text currentTimeUI;
    public Slider completionState;

    public WaitAnimation waitAnimation;
    public WindowAnimation windowAnim;
    public GameObject debugInfoPrefab;
    public Transform debugInfoCountainer;
    private ExerciseTracker tracker;
    private TimeSpan currentRemaining;
    private bool startTrackingUpdate;
    private DateTime trackingStart;
    private TimedTask currentTask;
    private ActivityAnalyser.Report currentReport;
    private List<GameObject> infoObjects = new List<GameObject>();

    private Action<ExerciseTrackingReport> onCompleteEvent;

    public static void ShowWaitingWindow(string exerciceDescription, TimedTask task, ExerciseTracker tracker, Action<ExerciseTrackingReport> onComplete = null, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        Scenes.LoadAsync(SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene) {
            scene.FindRootObject<TrackingWindow>().InitDisplay(exerciceDescription, task, tracker, onComplete, enAttente,title);
        });
    }

    private void Awake()
    {
        startTrackingUpdate = false;
        currentRemaining = new TimeSpan(0, 0, 0);
    }

    public void InitDisplay(string exerciceDescription, TimedTask task, ExerciseTracker tracker, Action<ExerciseTrackingReport> onComplete = null, string enAttente = "En Attente...", string title = "Faire l'exercice")
    {
        this.title.text = title;
        exercice.text = exerciceDescription;
        this.enAttente.text = enAttente;
        onCompleteEvent = onComplete;
        this.tracker = tracker;
        trackingStart = DateTime.Now;
        currentTask = task;
        windowAnim.Open(delegate() {
            startTrackingUpdate = true;
            waitAnimation.DoAnimation();
        });
    }

    public void AddDebugInfos(List<string> infos)
    {
        foreach (string info in infos)
        {
            GameObject newDebugInfo = Instantiate(debugInfoPrefab, debugInfoCountainer);
            infoObjects.Add(newDebugInfo);
            newDebugInfo.GetComponent<Text>().text = info;
        }
    }

    public void AddDebugInfo(string info)
    {
        GameObject newDebugInfo = Instantiate(debugInfoPrefab, debugInfoCountainer);
        newDebugInfo.GetComponent<Text>().text = info;
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
            if (currentReport.complete)
                Hide(ActivityAnalyser.ProduceReport(currentReport, ExerciseTrackingReport.State.Completed)); // exercise complete ! state=ExerciseTrackingReport.State 
            else
                UpdateExerciceCompletion(currentReport.timeSpendingExercice, new TimeSpan(0, (int)((WalkTask)currentReport.task.task).minutesOfWalk, 0));
        }
    }

    public void Hide(ExerciseTrackingReport trackingReport)
    {
        windowAnim.Close(delegate() {
            Scenes.UnloadAsync(SCENE_NAME);
            if (onCompleteEvent != null)
                onCompleteEvent.Invoke(trackingReport);
        });
    }
    
	public void ForceComplete()
    {
        currentReport = tracker.ForceCompletion(currentTask);
        Hide(ActivityAnalyser.ProduceReport(currentReport, ExerciseTrackingReport.State.UserSaidItWasCompleted));
    }

    public void ForceStop()
    {
        currentReport = tracker.ForceCompletion(currentTask);
        Hide(ActivityAnalyser.ProduceReport(currentReport, ExerciseTrackingReport.State.Stopped));
    }

    private void UpdateTimeExercice()
    {
        // on assume ici que l'activité dépend du temps
        currentRemaining = (DateTime.Now.Subtract(trackingStart));
        if (currentRemaining.CompareTo(new TimeSpan(0, 0, 0)) < 0)
            currentRemaining = new TimeSpan(0, 0, 0);
        currentTimeUI.text = "" + currentRemaining.Minutes.ToString() + ":" + currentRemaining.Seconds.ToString();
    }

    private void UpdateExerciceCompletion(TimeSpan timeDone, TimeSpan timeToDo)
    {
        int totalTimeDone = (timeDone.Hours * 60 * 60) + timeDone.Minutes * 60 + timeDone.Seconds; // secondes
        int totalTimeToDo = (timeToDo.Hours * 60 * 60) + timeToDo.Minutes * 60 + timeToDo.Seconds; // secondes
        float completion = totalTimeDone / totalTimeToDo;
        if (completionState != null)
            completionState.value = completion;
        currentTimeUI.text = "" + timeDone.Minutes.ToString() + ":" + timeDone.Seconds.ToString();
    }
}
