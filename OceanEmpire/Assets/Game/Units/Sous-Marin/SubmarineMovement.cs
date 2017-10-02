using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour, Interfaces.IClickInputs
{
    public float deadZoneRadius = 0.75f;

    //Proportionnal to the acceleration rate
    public float accelerationRate;
    //Maximium attainable speed
    public float maximumSpeed;

    public float distanceFromBound;
    public float leftBound;
    public float rightBound;

    private float upBound;
    private float downBound;
    private float deadZoneRadiusSQR;



    public float brakeDistance = 1.5f;

    public Vector2 currentTarget;

    private Rigidbody2D rb;
    //private float 

    // Use this for initialization

    void Start()
    {
        deadZoneRadiusSQR = deadZoneRadius * deadZoneRadius;
        rb = GetComponent<Rigidbody2D>();
        currentTarget = new Vector2(transform.position.x, transform.position.y);

        Game.OnGameStart += Init;
    }

    void Init()
    {
        MapInfo m = Game.instance.map;
        upBound = m.mapTop;
        downBound = m.mapBottom;
        return;
    }


    void FixedUpdate()
    {

        Vector2 distance = currentTarget - (Vector2)transform.position;
        Vector2 direction = distance.normalized;

        Vector2 targetSpeed;

        if (distance.magnitude < brakeDistance)
        {
            targetSpeed = distance.Capped(Vector2.one * maximumSpeed);
        }
        else
        {
            targetSpeed = direction * maximumSpeed;
        }

        Vector2 Speed = Vector2.Lerp(
            rb.velocity,
            targetSpeed,
            FixedLerp.Fix(0.1f * accelerationRate));
        rb.velocity = Speed;

    }


    public void OnClick(Vector2 position)
    {
        if((position - rb.position).sqrMagnitude> deadZoneRadiusSQR)
        {
            float d = distanceFromBound;

            currentTarget.x = position.x.Clamped(leftBound + d, rightBound - d);
            currentTarget.y = position.y.Clamped(downBound + d, upBound - d);
        }
    }
}
