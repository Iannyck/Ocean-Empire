using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectSnap : MonoBehaviour
{
    public enum Direction { Vertical, Horizontal }
    public Direction SnapDirection = Direction.Vertical;
    public RectTransform MovingRect;

    public bool useCustomPositionFunction = false;

    [Header("Standard Position Formula")]
    public float baseSnapDelta = 0;
    public float snapInterval = 100;

    [Header("Acceleration"), Forward]
    public GuidedAccelerationHandler accelerationHandler = new GuidedAccelerationHandler(1000);

    /// <summary>
    /// Doit retourner la anchoredPosition.y cible
    /// </summary>
    public Func<float> customSnapFunction;

    private ScrollRect ScrollRect
    {
        get { return _scrollRect ?? (_scrollRect = GetComponent<ScrollRect>()); }
    }
    private ScrollRect _scrollRect;

    void Update()
    {
        if (MovingRect != null)
        {
            var targetPos = 0f;
            var currentPos = GetCurrentPos();
            if (useCustomPositionFunction && customSnapFunction != null)
            {
                targetPos = customSnapFunction();
            }
            else
            {
                targetPos = (currentPos- baseSnapDelta).RoundedTo(snapInterval) + baseSnapDelta;
            }

            var deltaTime = Time.deltaTime;
            accelerationHandler.UpdateAcceleration(targetPos, currentPos, GetVelocity(), deltaTime);
            ScrollRect.velocity += GetPositiveVelocity() * accelerationHandler.CurrentAcceleration * deltaTime;
        }
    }

    float GetCurrentPos()
    {
        if (SnapDirection == Direction.Vertical)
            return MovingRect.anchoredPosition.y;
        else
            return MovingRect.anchoredPosition.x;
    }

    float GetVelocity()
    {
        if (SnapDirection == Direction.Vertical)
            return ScrollRect.velocity.y;
        else
            return ScrollRect.velocity.x;
    }

    Vector2 GetPositiveVelocity()
    {
        if (SnapDirection == Direction.Vertical)
            return Vector2.up;
        else
            return Vector2.right;
    }
}
