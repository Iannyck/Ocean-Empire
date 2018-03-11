using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript_Fred : MonoBehaviour
{
    public PalierPlans palierManager;
    public PalierContentManager contentManager;
    public Camera cam;


    private void Awake()
    {
    }

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        int palierIndex = palierManager.GetClosestPalier(point.y);
        Debug.DrawLine(point, Vector3.up * palierManager.GetPalierCenter(palierIndex));

        if (Input.GetKeyDown(KeyCode.P))
        {
            contentManager.CenterPalier--;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            contentManager.CenterPalier++;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            contentManager.CenterPalier -= 5;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            contentManager.CenterPalier += 5;
        }
    }
}