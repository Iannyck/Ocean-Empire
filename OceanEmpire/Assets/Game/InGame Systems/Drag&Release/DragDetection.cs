using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragDetection : MonoBehaviour
{
    [System.Serializable]
    public class DragEvent: UnityEvent<Vector2> { }

    public float dragMinScreenPercentage = 0.01f;
    public DragEvent onStartDrag = new DragEvent();
    public DragEvent onReleaseDrag = new DragEvent();

    [ReadOnly]
    public float dragMinSQRDist = 5;

    [ReadOnly]
    public DragProfile drag = DragProfile.Null;

    private void Awake()
    {
        dragMinSQRDist = Screen.height * dragMinScreenPercentage;
        dragMinSQRDist *= dragMinSQRDist;
    }

    void Update()
    {
        Vector2 touchPos;
        bool isTouching = GetTouchPosition(out touchPos);

        DragProfile.State wasState = drag.state;

        if (isTouching)
        {
            if (drag.state == DragProfile.State.Released)
            {
                drag = new DragProfile(touchPos);
            }
            else
            {
                drag.UpdateState(touchPos, dragMinSQRDist);
            }
        }
        else
        {
            drag.UpdateState();
        }

        DragProfile.State newState = drag.state;

        if(wasState == DragProfile.State.Pressed && newState == DragProfile.State.Dragged)
        {
            onStartDrag.Invoke(touchPos);
        }
        else if (wasState == DragProfile.State.Dragged && newState != DragProfile.State.Dragged)
        {
            onReleaseDrag.Invoke(touchPos);
        }
    }

    public static bool GetTouchPosition(out Vector2 position)
    {
        bool touching = false;
        if (Input.touchSupported)
        {
            touching = Input.touchCount > 0;
            if (touching)
                position = Input.GetTouch(0).position;
            else
                position = Vector2.zero;
        }
        else
        {
            touching = Input.GetMouseButton(0);
            if (touching)
                position = Input.mousePosition;
            else
                position = Vector2.zero;
        }
        return touching;
    }
}

[System.Serializable]
public struct DragProfile
{
    public Vector2 startPosition;
    public Vector2 currentPosition;
    public enum State { Pressed = 0, Dragged = 1, Released = 2 }
    public State state;

    public DragProfile(Vector2 touch)
    {
        startPosition = touch;
        currentPosition = startPosition;
        state = State.Pressed;
    }

    public void UpdateState(Vector2 touch, float dragMinSQRDist)
    {
        switch (state)
        {
            case State.Pressed:
                if ((currentPosition - startPosition).sqrMagnitude > dragMinSQRDist)
                {
                    state = State.Dragged;
                }
                break;
            case State.Released:
                startPosition = touch;
                state = State.Pressed;
                break;
        }
        currentPosition = touch;
    }

    public void UpdateState()
    {
        state = State.Released;
    }

    public static DragProfile Null
    {
        get
        {
            return new DragProfile()
            {
                state = State.Released
            };
        }
    }
}
