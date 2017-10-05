using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPalier{

    public event SimpleEvent palierDespawnEvent;
    public bool isActive = false;

    public static float repopulationCycle;

    private float lastActivationTime;

    private float AbsoluteFishLimit;
    private float currentFishLimit;

    public void Init(float limit)
    {
        currentFishLimit = AbsoluteFishLimit = limit;
        lastActivationTime = 0;
    }

    public void UpdateActivationTime(float aTime)
    {
        lastActivationTime = aTime;
    }

    public void Despawn(float time)
    {
        if (palierDespawnEvent != null)
            palierDespawnEvent();

        lastActivationTime = time;
    }
     
    public void SuscribeFish(BaseFish fish)
    {
        fish.captureEvent += CaptureFish;
    }

    public void UnsuscribeFish(BaseFish fish)
    {
        fish.captureEvent -= CaptureFish;
    }




    public float GetFishDensity(float currentTime)
    {
        float regenRate = ((currentTime - lastActivationTime) / repopulationCycle).Capped(1.0f);

        int additionnalFishes = (int)(AbsoluteFishLimit * regenRate).Raised(0.0f);

        currentFishLimit = (currentFishLimit + additionnalFishes).Capped(AbsoluteFishLimit);

        return currentFishLimit;
    }



    public void CaptureFish(BaseFish fish)
    {
        currentFishLimit -= (currentFishLimit > 0 ? 1 : 0);

    }
}
