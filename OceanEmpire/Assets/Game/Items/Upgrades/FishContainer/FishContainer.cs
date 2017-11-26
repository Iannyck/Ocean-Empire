﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Item/Fish Container")]
public class FishContainer : Upgrade {


    public float fishCapacity = 20;
    [SerializeField, ReadOnly]
    private float fishContained = 0;

    public float GetFishCapacity()
    {
        return fishCapacity;
    }

    public void TryCapture(BaseFish fish)
    {
        if (fishContained < fishCapacity)
        {
            fish.Capture();
            fishContained += fish.description.populationValue;
        }
    }

    public void ResetContainedFish()
    {
        fishContained = 0;
    }
}