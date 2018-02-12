using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectComponent : MonoBehaviour {

    // GENERIC START UP 

    public enum BehaviorStartUpMoment
    {
        onAwake = 0,
        onStart = 1,
        onDelay = 2,
        onEvent = 3
    }

    public BehaviorStartUpMoment moment;

    // start up with delay
    public float delay = 0f;
    public bool alwaysApplyDelay = false;

    // start up with event
    public event SimpleEvent startEffect;

    protected virtual void Awake()
    {
        if(moment == BehaviorStartUpMoment.onAwake)
            StartEffect();
    }

    void Start()
    {
        switch (moment)
        {
            case BehaviorStartUpMoment.onAwake:
                break;
            case BehaviorStartUpMoment.onStart:
                StartEffect();
                break;
            case BehaviorStartUpMoment.onDelay:
                this.DelayedCall(StartEffect, delay);
                break;
            case BehaviorStartUpMoment.onEvent:
                startEffect += StartEffect;
                break;
            default:
                break;
        }
    }

    protected virtual void StartEffect() { }
}
