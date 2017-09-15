using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInputs : MonoBehaviour {

    public UnityEvent screenTouched = new UnityEvent();
    public UnityEvent screenClicked = new UnityEvent();

    public MonoBehaviour externalScriptTouch;
    public MonoBehaviour externalScriptMouse;

    // Update is called once per frame
    void Update ()
    {
		if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Get movement of the finger since last frame
            Vector2 touchDeltaPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            screenTouched.Invoke();

            if (externalScriptTouch == null)
                return;

            // Send Position to a script and trigger the event
            if (externalScriptTouch.GetComponent<Interfaces.ITouchInputs>() != null)
                externalScriptTouch.GetComponent<Interfaces.ITouchInputs>().OnTouch(touchDeltaPosition);
            else if(GetComponent<Interfaces.ITouchInputs>() != null)
                GetComponent<Interfaces.ITouchInputs>().OnTouch(touchDeltaPosition);

        } else if(Input.GetMouseButtonDown(0))
        {
            Vector2 touchDeltaPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            screenClicked.Invoke();

            if (externalScriptMouse == null)
                return;

            // Send Position to a script and trigger the event
            if (externalScriptMouse.GetComponent<Interfaces.IClickInputs>() != null)
                externalScriptMouse.GetComponent<Interfaces.IClickInputs>().OnClick(touchDeltaPosition);
            else if(GetComponent<Interfaces.IClickInputs>() != null)
                GetComponent<Interfaces.IClickInputs>().OnClick(touchDeltaPosition);
        }
	}
}
