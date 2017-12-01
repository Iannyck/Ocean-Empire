using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cadre : MonoBehaviour {

    public GameObject cadrePrefab;

    private Camera mainCam;

	// Use this for initialization
	void Start ()
    {
        if(mainCam == null)
        {
            if (GetComponent<Camera>() != null)
                mainCam = GetComponent<Camera>();
            else
                mainCam = Camera.main;
        }
    }
	
	void Update ()
    {
        cadrePrefab.gameObject.transform.position = mainCam.transform.position;
    }
}
