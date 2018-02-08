using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    DateTime a = new DateTime(2018, 2, 10, 0, 0, 0);
    DateTime b = new DateTime(2000, 2, 10, 0, 30, 0);
    DateTime c = new DateTime(2000, 2, 10, 1, 30, 0);
    DateTime d = new DateTime(2000, 2, 10, 3, 30, 0);

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var x = new TimeSlot(a, b);
            var y = new TimeSlot(c, d);
            Process(ref x, ref y);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            var x = new TimeSlot(a, c);
            var y = new TimeSlot(b, d);
            Process(ref x, ref y);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            var x = new TimeSlot(a, d);
            var y = new TimeSlot(b, c);
            Process(ref x, ref y);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            var x = new TimeSlot(c, d);
            var y = new TimeSlot(a, b);
            Process(ref x, ref y);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            var x = new TimeSlot(b, d);
            var y = new TimeSlot(a, c);
            Process(ref x, ref y);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            var x = new TimeSlot(b, c);
            var y = new TimeSlot(a, d);
            Process(ref x, ref y);
        }
    }

    private void Process(ref TimeSlot x, ref TimeSlot y)
    {
        TimeSlot ts;
        x.IsOverlappingWith(y, out ts);
        print(ts);
        x.start = new DateTime(500);
        if (x.start == ts.start)
            print("da fuck");
    }
}