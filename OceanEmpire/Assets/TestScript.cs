using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour, Interfaces.IClickInputs
{

    void Update()
    {

    }

    public void OnClick(Vector2 position)
    {
        Debug.Log("Test");
    }
}
