using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript_Fred : MonoBehaviour
{
    public CanvasGroupBehaviour g;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            g.Hide(() => print("hide complete"));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            g.Show(() => print("show complete"));
        }
    }
}
