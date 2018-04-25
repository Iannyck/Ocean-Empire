using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailureAdvice : MonoBehaviour {

    [SerializeField]
    private Text text;

    public float closeDelay = 5.0f;

    public void UpdateInfo(string advice)
    {
        text.text = advice;
        this.DelayedCall(delegate ()
        {
            GetComponent<WindowAnimation>().Close(delegate() {
                PlannedExerciceRewarder.instance.keepAnalysing = true;
            });
        }, closeDelay,true);
    }
}
