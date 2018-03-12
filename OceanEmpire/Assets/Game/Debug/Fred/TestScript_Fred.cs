using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        //var t = ScriptableObject.CreateInstance<FishRepartition>();
    }
}