using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class KeyPressRecorder : CCC.Persistence.MonoPersistent
{
    public override void Init(Action onComplete)
    {
        onComplete();
        instance = this;
    }

    [SerializeField] private KeyCode recordedKeycode = KeyCode.E;
    private List<DateTime> presses = new List<DateTime>();


    public static KeyPressRecorder instance;

    void Update()
    {
        if (Input.GetKeyDown(recordedKeycode))
        {
            presses.Add(DateTime.Now);
            Debug.Log("key press exercise !");
        }
    }

    public ReadOnlyCollection<DateTime> GetKeyPresses()
    {
        return presses.AsReadOnly();
    }
}
