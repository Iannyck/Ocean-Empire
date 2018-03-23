using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Math.Graph;

public class TestScript_Fred : MonoBehaviour
{
    public DataSaver dataSaver;

    private void Awake()
    {
        Debug.Log("awake script : " + dataSaver.name);
    }

    private void OnEnable()
    {
        Debug.Log("onEnable script" + dataSaver.name);
    }

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }
}