using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float defaultSpeed;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Shoot_Velocity(Vector2 velocity)
    {
        rb.velocity = velocity;
    }
    public void Shoot_Direction(Vector2 direction)
    {
        Shoot_Velocity(direction.normalized * defaultSpeed);
    }
}
