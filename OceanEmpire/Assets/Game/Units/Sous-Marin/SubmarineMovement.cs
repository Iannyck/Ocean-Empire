using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour
{
    [Header("Components"), SerializeField] private DragDetection dragDetection;

    [Header("Enables")]
    public bool inputEnable = true;
    public bool movementEnable = true;

    [Header("Move Settings")]
    public float accelerationRate;
    public float maximumSpeed;
    public float brakeDistance = 1.5f;
    [ReadOnly] public Vector2 currentTarget;

    [Header("Bounds")]
    public bool clampPosition = true;
    public float distanceFromBound;
    public float leftBound;
    public float rightBound;


    private float upBound = float.MaxValue;
    private float downBound = float.MinValue;

    private Rigidbody2D rb;
    private float realBrakeDistance = -1;

    private ThrusterDescription thruster;
    private SlingshotControl slingshotControl;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        realBrakeDistance = brakeDistance;
    }

    void Start()
    {
        currentTarget = new Vector2(transform.position.x, transform.position.y);

        Game.OnGameStart += Init;

        slingshotControl = GetComponent<SlingshotControl>();

        SubmarinParts parts = gameObject.GetComponent<SubmarinParts>();

        thruster = parts.GetThruster();
        if (thruster != null)
        {
            maximumSpeed = thruster.GetSpeed();
            accelerationRate = thruster.GetAcceleration();
            //decelerationRate thruster.GetDeceleration();
        }

    }

    void Init()
    {
        MapInfo m = Game.Instance.map;
        upBound = m.mapTop;
        downBound = m.mapBottom;
    }

    void Update()
    {
        UpdateTargetPosition();
    }
    void FixedUpdate()
    {
        if (clampPosition)
            rb.position = ClampPosition(rb.position);

        if (movementEnable == false)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 distance = currentTarget - (Vector2)transform.position;
        Vector2 direction = distance.normalized;

        Vector2 targetSpeed;

        if (distance.magnitude < realBrakeDistance)
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

    protected Vector2 ClampPosition(Vector2 pos)
    {
        pos.x = pos.x.Clamped(leftBound + distanceFromBound, rightBound - distanceFromBound);
        pos.y = pos.y.Clamped(downBound + distanceFromBound, upBound - distanceFromBound);
        return pos;
    }

    public void UpdateTargetPosition()
    {
        if (dragDetection.IsTouching && inputEnable && !dragDetection.OriginatedInDeadZone && !slingshotControl.isDragging)
        {
            var worldPos = dragDetection.LastWorldTouchedPosition;
            float sqrDist;
            if (!dragDetection.IsWithinDeadZone(worldPos, out sqrDist))
            {
                currentTarget = ClampPosition(worldPos);

                realBrakeDistance = (0.2f + sqrDist * 0.5f).Capped(brakeDistance);
            }
        }
    }

    public void PushAwayFromTarget(float duration, float speedBoost, float force)
    {
        inputEnable = false;
        Vector2 displacement = currentTarget - (Vector2)transform.position;

        float minDistance = 1;
        if (displacement.sqrMagnitude < (minDistance * minDistance))
            displacement = (displacement.normalized) * minDistance;

        currentTarget = (Vector2)transform.position + (displacement * -1 * force);

        maximumSpeed += speedBoost;

        TransformFlipper flipper = GetComponentInChildren<TransformFlipper>();
        if (flipper != null)
            flipper.enabled = false;

        TransformTilter tilter = GetComponentInChildren<TransformTilter>();
        if (tilter != null)
            tilter.enabled = false;

        this.DelayedCall(delegate ()
        {
            maximumSpeed -= speedBoost;
            inputEnable = true;
            flipper.enabled = true;
            tilter.enabled = true;
        }, duration);
    }
}
