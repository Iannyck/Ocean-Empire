using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add this component anywhere and it will follow the camera
public class FollowCamera : MonoBehaviour {

    public Camera mainCamera;
    public Vector3 startPosition;

    void Start ()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        startPosition = gameObject.transform.position;
    }
	
	void Update ()
    {
        if (Camera.main != mainCamera)
            mainCamera = Camera.main;

        gameObject.transform.position = mainCamera.transform.position + startPosition;
	}
}
