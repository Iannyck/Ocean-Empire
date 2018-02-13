using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float defaultSpeed;
    public bool shootOnStart = true;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        if (shootOnStart)
            Shoot_Direction(transform.right);
    }

    public void Shoot_Velocity(Vector2 velocity)
    {
        transform.rotation = Quaternion.Euler(Vector3.forward * velocity.ToAngle());
        rb.velocity = velocity;
    }
    public void Shoot_Direction(Vector2 direction)
    {
        Shoot_Velocity(direction.normalized * defaultSpeed);
    }
}
