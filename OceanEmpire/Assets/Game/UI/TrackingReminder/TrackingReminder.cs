using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;

public class TrackingReminder : MonoBehaviour
{
    //La tache qu'on display presentement (peut être null)
    private ScheduledTask currentlyShownTask;

    // On écoute aux update du calendrier
    private void OnEnable()
    {
        if (Calendar.instance != null)
            Listen();
        else
            CCC.Manager.MasterManager.Sync(Listen);
    }

    private void Listen()
    {
        CheckCalendar(true);
        Calendar.instance.onTaskAdded += CheckCalendar;
        Calendar.instance.onTaskConcluded += CheckCalendar;
    }

    // On écoute aux update du calendrier
    private void OnDisable()
    {
        if (Calendar.instance != null)
        {
            Calendar.instance.onTaskAdded -= CheckCalendar;
            Calendar.instance.onTaskConcluded -= CheckCalendar;
        }
    }

    //Mise a jour du calendrier
    void CheckCalendar() { CheckCalendar(false); }
    void CheckCalendar(bool forceUpdate)
    {

        ScheduledTask taskWeShouldLookAt = null;

        if (!TrackingWindow.IsTrackingSomething())
        {
            ReadOnlyCollection<ScheduledTask> scheduledTasks = Calendar.instance.GetScheduledTasks();

            for (int i = 0; i < scheduledTasks.Count; i++)
            {
                ScheduledTask task = scheduledTasks[i];
                if (task.timeSlot.IsInTheFuture())
                    break;

                if (task.timeSlot.IsNow())
                {
                    taskWeShouldLookAt = task;
                    break;
                }
            }
        }

        if(taskWeShouldLookAt != currentlyShownTask || forceUpdate)
        {
            SetTask(taskWeShouldLookAt);
        }
    }

    void SetTask(ScheduledTask task)
    {
        if (task == null)
            Hide();
        else
            Show(task);
    }

    void Show(ScheduledTask task)
    {
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
