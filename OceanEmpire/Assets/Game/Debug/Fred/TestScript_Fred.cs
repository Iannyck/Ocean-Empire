using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CCC.Math.Graph;

public class TestScript_Fred : MonoBehaviour
{
    public GraphDrawer graphDrawer;
    public Rigidbody cube;

    private ColoredCurve curve = new ColoredCurve(Color.magenta);

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
        graphDrawer.curves.Add(curve);
    }

    private void Update()
    {
        curve.Positions.Add(new Vector2(Time.timeSinceLevelLoad, cube.position.y));
    }

    void OnDrawGizmos()
    {
        graphDrawer.Draw();
    }

    void OnRenderObject()
    {
        graphDrawer.Draw();
    }
}