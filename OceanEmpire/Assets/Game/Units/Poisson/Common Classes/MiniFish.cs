using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniFish : BaseFish
{

    public void OnPlayerTouch()
    {
        Game.PlayerStats.TryCapture(this);
        //Capture();
    }
}