using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perfomance_Spinner : MonoBehaviour
{
    public float speed = 20;
    void Update()
    {
        Quaternion rot = transform.rotation;
        Vector3 angles = rot.eulerAngles;
        transform.rotation = Quaternion.Euler(angles.x, angles.y, angles.z + Time.deltaTime * speed);
    }
}
