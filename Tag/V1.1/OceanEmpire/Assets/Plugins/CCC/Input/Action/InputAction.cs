using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Input.Action;


[CreateAssetMenu(menuName = "CCC/Input/Input Action", fileName = "IA_NewAction")]
public class InputAction : ScriptableObject
{
    [Header("Touch")]
    public bool screenTouch = false;

    [Header("Mouse"), Tooltip("0=left    1=right    2=middle")]
    public List<int> mouseButtons = new List<int>();

    [Header("Keys")]
    public List<KeyCombination> keyCombinations = new List<KeyCombination>();

    public bool Get()
    {
        //Test screen touch
        if (screenTouch && UnityEngine.Input.touchCount > 0)
        {
            return true;
        }

        //Test mouse buttons
        if (mouseButtons != null)
            for (int i = 0; i < mouseButtons.Count; i++)
            {
                if (UnityEngine.Input.GetMouseButton(mouseButtons[i]))
                    return true;
            }

        //Test key combinations
        if (keyCombinations != null)
            for (int i = 0; i < keyCombinations.Count; i++)
            {
                if (keyCombinations[i].Get())
                    return true;
            }

        return false;
    }

    public bool GetDown()
    {
        //Test screen touch
        if (screenTouch && UnityEngine.Input.touchCount > 0)
        {
            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                if (UnityEngine.Input.GetTouch(i).phase == TouchPhase.Began)
                    return true;
            }
        }

        //Test mouse buttons
        if (mouseButtons != null)
            for (int i = 0; i < mouseButtons.Count; i++)
            {
                if (UnityEngine.Input.GetMouseButtonDown(mouseButtons[i]))
                    return true;
            }

        //Test key combinations
        if (keyCombinations != null)
            for (int i = 0; i < keyCombinations.Count; i++)
            {
                if (keyCombinations[i].GetDown())
                    return true;
            }

        return false;
    }

    public bool GetUp()
    {
        //Test screen touch
        if (screenTouch && UnityEngine.Input.touchCount > 0)
        {
            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                TouchPhase phase = UnityEngine.Input.GetTouch(i).phase;
                if (phase == TouchPhase.Ended || phase == TouchPhase.Canceled)
                    return true;
            }
        }

        //Test mouse buttons
        if (mouseButtons != null)
            for (int i = 0; i < mouseButtons.Count; i++)
            {
                if (UnityEngine.Input.GetMouseButtonUp(mouseButtons[i]))
                    return true;
            }

        //Test key combinations
        if (keyCombinations != null)
            for (int i = 0; i < keyCombinations.Count; i++)
            {
                if (keyCombinations[i].GetUp())
                    return true;
            }

        return false;
    }
}

