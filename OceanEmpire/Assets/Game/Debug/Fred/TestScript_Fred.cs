using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript_Fred : MonoBehaviour
{
    public FishingFrenzyActivatedPopup popup;

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            popup.Animate();
        }
    }
}