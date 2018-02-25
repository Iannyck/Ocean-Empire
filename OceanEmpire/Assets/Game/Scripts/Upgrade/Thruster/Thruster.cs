using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Thruster : Upgrade {

    public float speed = 1;
    public float acceleration = 1;
    public float deceleration = 1;

    public float GetSpeed()
    {
        return speed;
    }
    public float GetAcceleration()
    {
        return acceleration;
    }


    public Thruster(float _speed = 1, float _acceleration = 1, float _decceleration = 1)
    {
        this.speed = _speed;
        this.acceleration = _acceleration;
        this.deceleration = _decceleration;
    }
}
