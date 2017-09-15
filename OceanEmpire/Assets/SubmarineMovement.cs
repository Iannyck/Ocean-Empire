using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineMovement : MonoBehaviour {

    public float acceleration;
    public float maximumSpeed;

    public Rigidbody2D rb;

    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("space"))
            SetDestination(new Vector3(0, 0, 0));
    }


    void SetDestination(Vector3 dest)
    {
        rb.AddForce((transform.position - dest) * 10);
     
    }
}
