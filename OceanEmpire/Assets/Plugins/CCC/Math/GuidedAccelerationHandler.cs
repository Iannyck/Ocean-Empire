using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GuidedAccelerationHandler
{
    public float CurrentAcceleration { get; private set; }
    public float MaxTargetAcceleration;

    public GuidedAccelerationHandler(float maxTargetAcceleration)
    {
        MaxTargetAcceleration = maxTargetAcceleration;
        CurrentAcceleration = 0;
    }
    public void UpdateAcceleration(float targetPosition, float currentPosition, float currentSpeed, float deltaTime)
    {
        var dD = targetPosition - currentPosition;
        var maxAcc = (((dD / deltaTime) - currentSpeed) / deltaTime).Abs();
        var standardAcc = dD.Sign() * MaxTargetAcceleration.Abs();

        //Si on ne va pas dans la bonne direction, on accelere dans la bonne
        if (currentSpeed.Sign() != dD.Sign())
        {
            CurrentAcceleration = standardAcc;
        }
        else
        {
            var a = -(currentSpeed * currentSpeed) / (2 * dD);
            float borne = standardAcc.Abs();
            if (a.Abs() >= borne)
            {
                //On ralentie
                CurrentAcceleration = a.Clamped(-borne, borne);
            }
            else
            {
                //On accelère
                CurrentAcceleration = standardAcc;
            }
        }

        //On clamp l'acceleration pour etre certain de ne pas dépasser la cible en 1 frame
        CurrentAcceleration = CurrentAcceleration.Clamped(-maxAcc, maxAcc);
    }
}
