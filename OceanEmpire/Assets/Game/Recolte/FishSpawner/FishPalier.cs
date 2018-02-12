using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPalier
{
    public event SimpleEvent palierDespawnEvent;
    public bool isActive = false;

    public static float repopulationCycle;

    private float lastActivationTime;

    private float AbsoluteFishLimit;
    private float currentFishLimit;

    public void InitLimit(float limit)
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
    public void SubscribeFish(PalierSubscriber subscriber)
    {
        var capturable = subscriber.GetComponent<Capturable>();
        if (capturable != null)
            capturable.OnNextCapture += OnFishCaptured;
    }

    public void UnsubscribeFish(PalierSubscriber subscriber)
    {
        var capturable = subscriber.GetComponent<Capturable>();
        if (capturable != null)
            capturable.OnNextCapture -= OnFishCaptured;
    }


    public float GetFishDensity(float currentTime)
    {
        float regenRate = ((currentTime - lastActivationTime) / repopulationCycle).Capped(1.0f);

        int additionnalFishes = (int)(AbsoluteFishLimit * regenRate).Raised(0.0f);

        currentFishLimit = (currentFishLimit + additionnalFishes).Capped(AbsoluteFishLimit);

        return currentFishLimit;
    }




    public void OnFishCaptured(Capturable capturable)
    {
        currentFishLimit -= (currentFishLimit > 0 ? 1 : 0);
    }
}
