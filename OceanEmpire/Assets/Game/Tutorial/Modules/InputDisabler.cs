using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InputDisabler : MonoBehaviour
{
    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
        EnableInput();
    }

    public void EnableInput()
    {
        image.enabled = false;
    }

    public void DisableInput()
    {
        image.enabled = true;
    }
}
