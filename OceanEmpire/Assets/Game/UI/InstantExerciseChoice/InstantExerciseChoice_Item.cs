using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantExerciseChoice_Item : MonoBehaviour
{
    public delegate void Event(InstantExerciseChoice_Item item);
    public Task assignedTask;
    public Event onClick;

    public void DisplayTask(Task task)
    {
        assignedTask = task;
    }

    public void Click()
    {
        if (onClick != null)
            onClick(this);
    }
}
