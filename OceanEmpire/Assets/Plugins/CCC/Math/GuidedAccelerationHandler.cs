using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuidedAccelerationHandler
{
    public float CurrentAcceleration { get; private set; }
    public float MaxTargetAcceleration;
    private const float CatchUpMultiplier = 1.1f;

    public GuidedAccelerationHandler(float maxTargetAcceleration)
    {
        MaxTargetAcceleration = maxTargetAcceleration;
        CurrentAcceleration = 0;
    }
    public void UpdateAcceleration(float targetPosition, float currentPosition, float currentSpeed, float deltaTime)
    {
        if (MaxTargetAcceleration <= 0 || deltaTime <= 0)
            return;

        var d = targetPosition - currentPosition;
        var a = MaxTargetAcceleration * d.Sign();
        var idealETA = Mathf.Sqrt(2 * d / a);

        float idealVelocity = 0;
        if (idealETA.Abs() < deltaTime)
        {
            // Nous ne devons pas dépacer la cible en 1 frame
            idealVelocity = d / deltaTime;
        }
        else
        {
            // Nous visons la vitesse moyenne entre la frame courrante et la frame suivante
            idealVelocity = (2 * idealETA - deltaTime) * a / 2;
        }

        var wantedAcceleration = (idealVelocity - currentSpeed) / deltaTime;
        CurrentAcceleration = Mathf.Clamp(wantedAcceleration, -MaxTargetAcceleration * CatchUpMultiplier, MaxTargetAcceleration * CatchUpMultiplier);
    }
}
