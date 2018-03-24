using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragDetection : MonoBehaviour
{
    [System.Serializable]
    public class DragEvent : UnityEvent<Vector2> { }

    public float dragMinScreenPercentage = 0.01f;
    public float deadZoneRadius = 0.75f;
    public DragEvent onStartDrag = new DragEvent();
    public DragEvent onReleaseDrag = new DragEvent();

    [ReadOnly]
    public float dragMinSQRDist = 5;

    [ReadOnly]
    public DragProfile drag = DragProfile.Null;

    public Vector2 LastWorldTouchedPosition { get { return lastWorldTouchedPosition; } }
    public bool IsTouching { get { return drag.state != DragProfile.State.Released; } }
    public bool LastTouchIsWithinDeadZone { get { return IsWithinDeadZone(lastWorldTouchedPosition); } }
    public bool OriginatedInDeadZone { get { return originatedInDeadZone; } }

    private Transform tr;
    private bool originatedInDeadZone = false;
    private Vector2 lastWorldTouchedPosition;

    private void Awake()
    {
        dragMinSQRDist = Screen.height * dragMinScreenPercentage;
        dragMinSQRDist *= dragMinSQRDist;
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        Vector2 touchPos;
        bool isTouching = GetTouchPosition(out touchPos);
        if (isTouching)
            lastWorldTouchedPosition = GetCamera().ScreenToWorldPoint(touchPos);

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

        if (wasState == DragProfile.State.Released && newState == DragProfile.State.Pressed)
        {
            // Just started to press
            originatedInDeadZone = IsWithinDeadZone(lastWorldTouchedPosition);
        }
        else if (wasState == DragProfile.State.Pressed && newState == DragProfile.State.Dragged)
        {
            // Just started drag
            onStartDrag.Invoke(drag.startPosition);
        }
        else if (wasState == DragProfile.State.Dragged && newState != DragProfile.State.Dragged)
        {
            // Just released drag
            onReleaseDrag.Invoke(drag.lastRecordedPosition);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawSphere(transform.position, deadZoneRadius);
    }

    public bool IsWithinDeadZone(Vector2 worldPoint)
    {
        float sqrDist;
        return IsWithinDeadZone(worldPoint, out sqrDist);
    }
    public bool IsWithinDeadZone(Vector2 worldPoint, out float sqrDistance)
    {
        sqrDistance = (worldPoint - (Vector2)tr.position).sqrMagnitude;
        return sqrDistance <= deadZoneRadius * deadZoneRadius;
    }

    Camera GetCamera()
    {
        if (Game.Instance != null)
            return Game.Instance.GameCamera.CameraComponent;
        else
            return GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    public static bool GetTouchPosition(out Vector2 screenPosition)
    {
        bool touching = false;
        if (Input.touchSupported)
        {
            touching = Input.touchCount > 0;
            if (touching)
                screenPosition = Input.GetTouch(0).position;
            else
                screenPosition = Vector2.zero;
        }
        else
        {
            touching = Input.GetMouseButton(0);
            if (touching)
                screenPosition = Input.mousePosition;
            else
                screenPosition = Vector2.zero;
        }
        return touching;
    }
}

[System.Serializable]
public struct DragProfile
{
    public Vector2 startPosition;
    public Vector2 lastRecordedPosition;
    public enum State { Pressed = 0, Dragged = 1, Released = 2 }
    public State state;

    public DragProfile(Vector2 touch)
    {
        startPosition = touch;
        lastRecordedPosition = startPosition;
        state = State.Pressed;
    }

    public void UpdateState(Vector2 touchPos, float dragMinSQRDist)
    {
        switch (state)
        {
            case State.Pressed:
                if ((lastRecordedPosition - startPosition).sqrMagnitude > dragMinSQRDist)
                {
                    state = State.Dragged;
                }
                break;
            case State.Released:
                startPosition = touchPos;
                state = State.Pressed;
                break;
        }
        lastRecordedPosition = touchPos;
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
