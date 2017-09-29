using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFish : BaseFish
{

    public void OnPlayerTouch(Collider2D collider)
    {
        //Self kill
        //Capture();

        captureEvent.Invoke();
    }
}