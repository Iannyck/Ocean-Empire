using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FishInfo)), DisallowMultipleComponent]
public class Capturable : MonoBehaviour
{
    public delegate void CapturableEvent(Capturable capturable);

    public event CapturableEvent OnAllCaptures;
    public event CapturableEvent OnNextCapture;
    [System.NonSerialized] public FishInfo info;

    void Awake()
    {
        info = GetComponent<FishInfo>();
    }

    public void Capture()
    {
        // Raise Events
        if (OnNextCapture != null)
            OnNextCapture(this);
        OnNextCapture = null;

        if (OnAllCaptures != null)
            OnAllCaptures(this);

        // Kill
        var killable = GetComponent<BaseKillableUnit>();
        if (killable != null)
            killable.Kill();
    }
}
