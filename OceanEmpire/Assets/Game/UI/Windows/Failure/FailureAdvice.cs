using CCC.UI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailureAdvice : MonoBehaviour
{

    [SerializeField]
    private Text text;

    public float closeDelay = 5.0f;

    public void Display(string advice, TweenCallback onComplete)
    {
        text.text = advice;
        GetComponent<WindowAnimation>().Open();

        this.DelayedCall(delegate ()
        {
            GetComponent<WindowAnimation>().Close(onComplete);
        }, closeDelay, true);
    }
}
