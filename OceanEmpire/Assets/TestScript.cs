using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            Lottery<A> lot = new Lottery<A>(elements.Length);
            for (int i = 0; i < elements.Length; i++)
            {
                lot.Add(elements[i]);
            }
            lot.RemoveAt(4);
            print(lot.Pick().value);
            print("total weight: " + lot.TotalWeight);
        }
    }
}


//trying out stuff
