using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public DayInspector g;

    void Start()
    {
        CCC.Manager.MasterManager.Sync();
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");


        //InstantExerciseChoice.ProposeTasks();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            InstantExerciseChoice.ProposeTasks();
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    bool result = Calendar.instance.AddScheduledTask(new ScheduledTask(WalkTask.Build(1), DateTime.Now.AddMinutes(-50)));
        //    print(result);
        //    if (result)
        //        Calendar.instance.test.SortedAdd(-2);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    bool result = Calendar.instance.AddScheduledTask(new ScheduledTask(WalkTask.Build(1), DateTime.Now.AddMinutes(-10)));
        //    print(result);
        //    if (result)
        //        Calendar.instance.test.SortedAdd(-1);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    bool result = Calendar.instance.AddScheduledTask(new ScheduledTask(WalkTask.Build(1), DateTime.Now));
        //    print(result);
        //    if (result)
        //        Calendar.instance.test.SortedAdd(0);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    bool result = Calendar.instance.AddScheduledTask(new ScheduledTask(WalkTask.Build(1), DateTime.Now.AddMinutes(10)));
        //    print(result);
        //    if (result)
        //        Calendar.instance.test.SortedAdd(1);
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    bool result = Calendar.instance.AddScheduledTask(new ScheduledTask(WalkTask.Build(1), DateTime.Now.AddMinutes(50)));
        //    print(result);
        //    if (result)
        //        Calendar.instance.test.SortedAdd(2);
        //}
    }
}
