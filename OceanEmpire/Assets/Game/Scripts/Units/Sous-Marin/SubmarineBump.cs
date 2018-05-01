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
    [SerializeField] float defaultDeactivatedDuration = 0;

    private const string CAN_ACCELERATE_KEY = "bmp";

    private float reactivationTimer = 0;
    private float standardDrag = 0;

    void Awake()
    {
        standardDrag = rigidbody.drag;
    }

    public void Bump(Vector2 force) { Bump(force, defaultDeactivatedDuration); }
    public void Bump(Vector2 force, float deactivationDuration)
    {
        reactivationTimer = deactivationDuration;
        rigidbody.AddForce(force, ForceMode2D.Impulse);

        EnterBumpState();
    }

    void Update()
    {
        if (IsBumped)
        {
            reactivationTimer -= Time.deltaTime;
            if(reactivationTimer <= 0 && rigidbody.velocity.magnitude < reactivVelocity)
            {
                ExitBumpState();
            }
        }
    }

    public bool IsBumped
    {
        get; private set;
    }

    void EnterBumpState()
    {
        IsBumped = true;

        // Lock movement
        submarineMovement.canAccelerate.LockUnique(CAN_ACCELERATE_KEY);
        submarineMovement.ClearTarget();

        // Tilt animators
        if (transformTilter != null)
            transformTilter.enabled = false;
        if (transformFlipper != null)
            transformFlipper.enabled = false;

        // Drag
        rigidbody.drag = bumpedLinearDrag;
    }

    void ExitBumpState()
    {
        IsBumped = false;

        // Unlock movement
        submarineMovement.canAccelerate.UnlockAll(CAN_ACCELERATE_KEY);

        // Tilt animators
        if (transformTilter != null)
            transformTilter.enabled = true;
        if (transformFlipper != null)
            transformFlipper.enabled = true;

        // Drag
        rigidbody.drag = standardDrag;
    }
}
