using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineBump : MonoBehaviour
{
    [Header("Components"), SerializeField] private TransformFlipper transformFlipper;
    [SerializeField] private TransformTilter transformTilter;
    [SerializeField] new private Rigidbody2D rigidbody;
    [SerializeField] private SubmarineMovement submarineMovement;

    [Header("Settings"), SerializeField] float bumpedLinearDrag = 5;
    [SerializeField] float reactivVelocity = 0.5f;
    [SerializeField] float reactivMaxDelay = 2;

    private const string CAN_ACCELERATE_KEY = "bmp";

    private float reactivationTimer = 0;
    private float standardDrag = 0;

    void Awake()
    {
        standardDrag = rigidbody.drag;
    }

    public void Bump(Vector2 force)
    {
        rigidbody.drag = bumpedLinearDrag;
        submarineMovement.canAccelerate.Lock(CAN_ACCELERATE_KEY);
        submarineMovement.ClearTarget();
        reactivationTimer = reactivMaxDelay;
        if (transformTilter != null)
            transformTilter.enabled = false;
        if (transformFlipper != null)
            transformFlipper.enabled = false;

        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (IsBumped)
        {
            if (rigidbody.velocity.magnitude < reactivVelocity)
                ReactivatePlayerMovement();

            reactivationTimer -= Time.deltaTime;
            if (!IsBumped)
                ReactivatePlayerMovement();
        }
    }

    public bool IsBumped
    {
        get { return reactivationTimer > 0; }
    }

    void ReactivatePlayerMovement()
    {
        reactivationTimer = -1;

        if (transformTilter != null)
            transformTilter.enabled = true;
        if (transformFlipper != null)
            transformFlipper.enabled = true;
        rigidbody.drag = standardDrag;
        submarineMovement.canAccelerate.Unlock(CAN_ACCELERATE_KEY);
    }
}
