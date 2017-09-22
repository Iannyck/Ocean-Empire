using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFish : MonoBehaviour
{
    public FishDescription description;

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
        Destroy(gameObject);
    }
}
