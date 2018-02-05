using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Item/Thruster")]
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
