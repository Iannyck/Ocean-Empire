using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

[CreateAssetMenu(fileName = "TUT_FirstMap", menuName = "Ocean Empire/Tutorial/First Map")]
public class TUT_FirstMap : BaseTutorial {

    protected override void OnStart(){}

    public void FocusOnSubmarine(Action OnComplete)
    {
        Debug.Log("FOCUS ON SUBAMARINUUUU");
        OnComplete();
    }

    public void FocusOnGaz(Action OnComplete)
    {
        Debug.Log("FOCUS ON gaz desu");
        OnComplete();
    }

    public void FocusOnFish(Action OnComplete)
    {
        Debug.Log("s tu vois ca, c pas normal");
    }

    public void FocusOnOption(Action OnComplete)
    {
        Debug.Log("FOCUS ON your mom gay");
        OnComplete();
    }
}
