using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour, Interfaces.IClickInputs
{
    [System.Serializable]
    public struct A : IWeight
    {
        public string value;
        public float weight;
        public float Weight { get { return weight; } }
    }
    public A[] elements;

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick(Vector2 position)
    {
        Debug.Log("Test");
    }
}
