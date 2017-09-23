using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformFlipper : MonoBehaviour
{
    private const float REPOS = 0.1f;

    public bool facingRight;
    public Rigidbody2D rb;
    public float flipDuration = 0.4f;
    public Ease flipEase = Ease.InOutSine;

    private Tween flipTween;

    private void Update()
    {
        if (rb != null)
        {
            if (facingRight)
            {
                if (rb.velocity.x < -REPOS)
                {
                    facingRight = false;
                    Flip();
                }
            }
            else
            {
                if (rb.velocity.x > REPOS)
                {
                    facingRight = true;
                    Flip();
                }
            }
        }
    }

    private bool flipToggle = false;

    void Flip()
    {
        if (flipTween == null)
        {
            flipTween = transform.DOBlendableRotateBy(Vector3.up * 180, flipDuration, RotateMode.LocalAxisAdd).SetAutoKill(false);
        }
        else
        {
            if (flipToggle)
                flipTween.PlayForward();
            else
                flipTween.PlayBackwards();

            flipToggle = !flipToggle;
        }
    }
}
