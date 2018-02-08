using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour {
    private SubmarineMovement submarine;
    public bool followPlayer = true;
    public float acceleration = 1;
    public float decelerationSpeedFactor = 1;

    private float verticalSpeed = 0;
    private Transform tr;

    public float Height
    {
        get { return tr.position.y; }
    }

    private void Awake()
    {
        tr = transform;
    }


    void FixedUpdate () {
        if (submarine == null && Game.Instance != null) { 
            submarine = Game.Instance.submarine;
            return;
        }
        if (followPlayer && submarine != null && submarine.gameObject.activeSelf)
        {
            float hg = Height;

            float playerHeight = submarine.transform.position.y;
            float delta = playerHeight - hg;

            float targetSpeed = delta.Sign() * float.PositiveInfinity;

            verticalSpeed = verticalSpeed.MovedTowards(targetSpeed, acceleration * Time.fixedDeltaTime);

            if (delta > 0)
                verticalSpeed = verticalSpeed.Capped(delta * decelerationSpeedFactor);
            else
                verticalSpeed = verticalSpeed.Raised(delta * decelerationSpeedFactor);


            SetToHeight(hg + (verticalSpeed * Time.fixedDeltaTime));

            verticalSpeed = (Height - hg) / Time.fixedDeltaTime;
        }
    }

    void SetToHeight(float y)
    {
        tr.position = new Vector3(0, y, 0);
    }
    
}
