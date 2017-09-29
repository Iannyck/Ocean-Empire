using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseFish : MonoBehaviour, IKillable
{
    public delegate void FishEvent(BaseFish fish);

    public FishDescription description;
    public event FishEvent captureEvent;
    public event FishEvent deathEvent;

    public bool HasBeenCaptured
    {
        get { return hasBeenCaptured; }
    }
    private bool hasBeenCaptured = false;
    private bool hasBeenKilled = false;

    public void Capture()
    {
        if (hasBeenCaptured)
            return;
        hasBeenCaptured = true;
        
        OnCapture();
    }

    protected virtual void OnCapture()
    {
        if (captureEvent != null)
            captureEvent(this);

        Kill();
    }

    public void Kill()
    {
        if (hasBeenKilled)
            return;
        hasBeenKilled = true;

        if (deathEvent != null)
            deathEvent(this);

        gameObject.SetActive(false);

        ClearEvents();
    }

    protected virtual void ClearEvents()
    {
        deathEvent = null;
        captureEvent = null;
    }

    public void RemiseEnLiberté()
    {
        hasBeenKilled = false;
        hasBeenCaptured = false;

        BaseFishDriver driver = GetComponent<BaseFishDriver>();
        if (driver != null)
            driver.ClearMind();
    }

    public bool IsDead()
    {
        return hasBeenCaptured;
    }
}
