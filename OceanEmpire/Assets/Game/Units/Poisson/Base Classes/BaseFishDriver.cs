using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseFishDriver : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public Vector2 Speed
    {
        get { return rb.velocity; }
        set { rb.velocity = value; }
    }
}
