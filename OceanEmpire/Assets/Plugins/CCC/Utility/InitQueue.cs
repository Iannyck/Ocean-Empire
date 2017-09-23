using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitQueue
{
    Action onComplete;
    int count = 0;
    private bool endSpecified = false;

    public InitQueue(Action onComplete)
    {
        this.onComplete = onComplete;
    }
    public Action Register()
    {
        count++;
        return OnCompleteAnyInit;
    }
    public void MarkEnd()
    {
        endSpecified = true;
        if (count <= 0)
            onComplete();
    }
    void OnCompleteAnyInit()
    {
        count--;
        if (count <= 0 && endSpecified)
            onComplete();
    }
}
