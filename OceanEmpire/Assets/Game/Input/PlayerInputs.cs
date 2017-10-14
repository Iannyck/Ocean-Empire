using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;
using UnityEngine.Events;

public class PlayerInputs : BaseBehavior {

    public bool useTouch = false;
    public bool useClick = true;

    [System.Serializable]
    public class InputEvent : UnityEvent<Vector2> { }

    [InspectorShowIf("useTouch")]
    public InputEvent screenTouched = new InputEvent();
    [InspectorShowIf("useClick")]
    public InputEvent screenClicked = new InputEvent();

    [InspectorShowIf("useTouch")]
    public UnityEvent inputTouched = new UnityEvent();
    [InspectorShowIf("useClick")]
    public UnityEvent inputClicked = new UnityEvent();

    [InspectorShowIf("useTouch")]
    public MonoBehaviour externalScriptTouch;
    [InspectorShowIf("useClick")]
    public MonoBehaviour externalScriptMouse;

    // Update is called once per frame
    void Update ()
    {
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            inputTouched.Invoke();
            screenTouched.Invoke(touchDeltaPosition);

            if (externalScriptTouch == null)
                return;

            // Send Position to a script and trigger the event
            if (externalScriptTouch.GetComponent<Interfaces.ITouchInputs>() != null)
                externalScriptTouch.GetComponent<Interfaces.ITouchInputs>().OnTouch(touchDeltaPosition);
            else if(gameObject.GetComponent<Interfaces.ITouchInputs>() != null)
                GetComponent<Interfaces.ITouchInputs>().OnTouch(touchDeltaPosition);

        } else if(Input.GetMouseButtonDown(0))
        {
            Vector2 clickDeltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            inputClicked.Invoke();
            screenClicked.Invoke(clickDeltaPosition);

            if (externalScriptMouse == null)
                return;

            // Send Position to a script and trigger the event
            if (externalScriptMouse.GetComponent<Interfaces.IClickInputs>() != null)
                externalScriptMouse.GetComponent<Interfaces.IClickInputs>().OnClick(clickDeltaPosition);
            else if(gameObject.GetComponent<Interfaces.IClickInputs>() != null)
                GetComponent<Interfaces.IClickInputs>().OnClick(clickDeltaPosition);
        }
	}
}
