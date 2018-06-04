using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputActionListener : MonoBehaviour
{
    public InputAction action;
    public bool onPressEvents = false;
    [SerializeField, HideInInspector]
    public UnityEvent onPress = new UnityEvent();

    public bool onHoldEvents = false;
    [SerializeField, HideInInspector]
    public UnityEvent onHold = new UnityEvent();

    public bool onReleaseEvents = false;
    [SerializeField, HideInInspector]
    public UnityEvent onRelease = new UnityEvent();

    private void Update()
    {
        if (onPressEvents && action.GetDown())
            onPress.Invoke();

        if (onHoldEvents && action.Get())
            onHold.Invoke();

        if (onReleaseEvents && action.GetUp())
            onRelease.Invoke();
    }
}