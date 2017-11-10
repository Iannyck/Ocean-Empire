using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : Upgrade {

    public float subMarineSpeed = 1;
    public float subMarineAcceleration = 1;

    public float GetSpeed()
    {
        return subMarineSpeed;
    }
    public float GetAcceleration()
    {
        return subMarineAcceleration;
    }
}
