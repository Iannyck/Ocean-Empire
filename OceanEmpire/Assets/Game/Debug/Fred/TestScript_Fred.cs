using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Math.Graph;

public class TestScript_Fred : MonoBehaviour
{
    public Color c;
    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }
    //00C0FFFF
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetComponent<Shack_Environment>().SetWaterColor(c);
        }
    }
}