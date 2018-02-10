using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    public RandomHSVColor randomColor;
    public Color color;

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            color = randomColor.GetRandomColor();
        }
    }
}