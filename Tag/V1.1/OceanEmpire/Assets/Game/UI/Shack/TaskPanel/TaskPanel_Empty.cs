using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel_Empty : MonoBehaviour, ITaskPanelState
{
    public void Enter(Action onComplete)
    {
        gameObject.SetActive(true);
        if (onComplete != null)
            onComplete();
    }

    public void Exit(Action onComplete)
    {
        gameObject.SetActive(false);
        if (onComplete != null)
            onComplete();
    }

    public void FillContent(object data)
    {
    }
}
