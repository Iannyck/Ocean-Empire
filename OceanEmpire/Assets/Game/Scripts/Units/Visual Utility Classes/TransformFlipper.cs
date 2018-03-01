using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformFlipper : MonoBehaviour
{
    private const float REPOS = 0.1f;

    [Header("Components"), SerializeField] Rigidbody2D rb;

    [Header("Settings"), SerializeField] bool startingRight = true;
    [SerializeField] float flipDuration = 0.4f;
    [SerializeField] Ease flipEase = Ease.InOutSine;
    public bool FlipManually = false;

    private Tween flipTween;
    private bool facingRight = false;

    void Start()
    {
        facingRight = startingRight;
    }

    private void Update()
    {
        if (!FlipManually && rb != null)
        {
            var vel = rb.velocity;
            if(vel.x.Abs() > REPOS)
            {
                FacingRight = vel.x > 0;
            }
        }
    }

    public bool FacingRight
    {
        get
        {
            return facingRight;
        }
        set
        {
            if (facingRight != value)
                Flip();
        }
    }

    private bool flipToggle = false;

    void Flip()
    {
        if (flipTween == null)
        {
            flipTween = transform.DOBlendableRotateBy(Vector3.up * 180, flipDuration, RotateMode.LocalAxisAdd)
                .SetEase(flipEase)
                .SetAutoKill(false);
        }
        else
        {
            if (flipToggle)
                flipTween.PlayForward();
            else
                flipTween.PlayBackwards();

            flipToggle = !flipToggle;
        }
        facingRight = !facingRight;
    }
}
