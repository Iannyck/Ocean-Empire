using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public event SimpleEvent a;
    public event SimpleEvent remoteA;
    public DayInspector g;

    void Start()
    {
        remoteA = a;
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            a += () => Debug.Log("hello");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            remoteA();
        }
    }
}
