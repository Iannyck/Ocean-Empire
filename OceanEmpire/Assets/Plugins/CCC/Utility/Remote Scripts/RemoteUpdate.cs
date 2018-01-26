using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteUpdate : MonoBehaviour
{
    public Action updateAction;

    void Update()
    {
        if (updateAction != null)
            updateAction();
    }
}
