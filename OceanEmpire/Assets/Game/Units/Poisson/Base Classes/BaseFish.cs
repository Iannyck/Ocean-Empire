using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseFish : MonoBehaviour
{
    public delegate void FishEvent(BaseFish fish);

    public FishDescription description;
    public event FishEvent captureEvent;


    public bool HasBeenCaptured
    {
        get { return hasBeenCaptured; }
    }
    private bool hasBeenCaptured = false;

    public void Capture()
    {
        /*
        if (hasBeenCaptured)
            return;
        hasBeenCaptured = true;
        */
        OnCapture();
    }

    protected virtual void OnCapture()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);

        if (captureEvent != null)
            captureEvent(this);
        captureEvent = null;
    }
}
