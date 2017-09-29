using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseFish : MonoBehaviour
{
    public FishDescription description;
    public UnityEvent captureEvent;


    public void Start()
    {
        captureEvent.AddListener(Capture);
    }


    public bool HasBeenCaptured
    {
        get { return hasBeenCaptured; }
    }
    private bool hasBeenCaptured = false;

    public void Capture()
    {
        
        if (hasBeenCaptured)
            return;
        hasBeenCaptured = true;

        OnCapture();
        
    }

    protected virtual void OnCapture()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
        captureEvent.RemoveAllListeners();
    }
}
