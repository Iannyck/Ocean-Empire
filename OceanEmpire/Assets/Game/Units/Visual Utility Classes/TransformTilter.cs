using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTilter : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxSpeed = 2;
    public float tiltSpeed = 10;
    [MinMaxSlider(-90, 90)]
    public Vector2 angleRange = new Vector2(-20, 20);

    private float maxSpeedSQR;
    private Transform tr;

    private void Awake()
    {
        maxSpeedSQR = maxSpeed;
        tr = transform;
    }

    private void Update()
    {
        if (rb != null)
        {
            Vector2 vel = rb.velocity;
            float sqrMag = vel.sqrMagnitude;

            float targetAngle = 0;
            if (sqrMag > 0.05f)
            {
                if (vel.x < 0)
                    vel = vel.FlippedX();
                float map = (vel.ToAngle() + 90) / 180;
                float influenceAngle = Mathf.Lerp(angleRange.x, angleRange.y, map);
                targetAngle = targetAngle.Lerpped(influenceAngle, sqrMag / maxSpeedSQR);
            }

            Quaternion rot = tr.rotation;
            Vector3 euler = rot.eulerAngles;
            tr.rotation = Quaternion.RotateTowards(
                tr.rotation,
                Quaternion.Euler(new Vector3(euler.x, euler.y, targetAngle)),
                tiltSpeed * Time.deltaTime);
        }
    }
}
