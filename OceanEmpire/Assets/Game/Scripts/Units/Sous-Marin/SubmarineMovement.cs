using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour
{
    [Header("Components"), SerializeField] DragDetection dragDetection;
    [SerializeField] TransformFlipper transformFlipper;

    [Header("Enables")]
    public Locker canReadInput = new Locker();
    public Locker canAccelerate = new Locker();

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

    private SlingshotControl slingshotControl;
    private bool hasATarget = false;
    private Transform tr;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = transform;
        realBrakeDistance = brakeDistance;
    }

    void Start()
    {
        Game.OnGameStart += OnGameStart;

        slingshotControl = GetComponent<SlingshotControl>();
    }

    void OnGameStart()
    {
        MapInfo m = Game.Instance.map;
        upBound = m.mapTop;
        downBound = m.mapBottom;
    }

    void Update()
    {
        if (canReadInput)
            ReadInput();
    }

    void FixedUpdate()
    {
        if (clampPosition)
            rb.position = ClampPosition(rb.position);

        if (!canAccelerate)
            return;

        var myPos = (Vector2)tr.position;
        var targetPos = hasATarget ? currentTarget : myPos;

        var v = targetPos - myPos;

        // Visually flip the submarine
        if(transformFlipper != null && v.x.Abs() > 0.1f)
        {
            transformFlipper.FacingRight = v.x > 0;
        }

        Vector2 targetSpeed;

        if (v.magnitude < realBrakeDistance)
        {
            targetSpeed = v.Capped(Vector2.one * maximumSpeed);
        }
        else
        {
            targetSpeed = v.normalized * maximumSpeed;
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

    private void ReadInput()
    {
        if (dragDetection.IsTouching && !dragDetection.OriginatedInDeadZone && !slingshotControl.isDragging)
        {
            var worldPos = dragDetection.LastWorldTouchedPosition;
            float sqrDist;
            if (!dragDetection.IsWithinDeadZone(worldPos, out sqrDist))
            {
                hasATarget = true;
                currentTarget = ClampPosition(worldPos);

                realBrakeDistance = (0.2f + sqrDist * 0.5f).Capped(brakeDistance);
            }
        }
    }

    public void ClearTarget()
    {
        hasATarget = false;
    }
}
