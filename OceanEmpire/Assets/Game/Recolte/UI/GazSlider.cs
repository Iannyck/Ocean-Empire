using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GazSlider : MonoBehaviour {

    public Slider gageMeter;

    void Start()
    {
        UpdateMeter();
    }

    void Update()
    {
        UpdateMeter();
    }

    public void UpdateMeter()
    {
        SubmarinParts parts;
        GazTank gazTank;
        if (Game.instance != null && (parts = Game.SubmarinParts) != null && (gazTank = parts.GetGazTank()) != null)
        {
            gageMeter.value = gazTank.GetGazRatio();
            Debug.Log(gazTank.GetGazRatio());

        }
        else
        { 
            gageMeter.value = 1;
            Debug.Log("wassa");
        }
    }
}
