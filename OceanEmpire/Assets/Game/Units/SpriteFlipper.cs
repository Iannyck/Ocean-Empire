using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlipper : MonoBehaviour
{
    private const float REPOS = 0.1f;

    public bool facingRight;
    public Rigidbody2D rb;
    public float flipDuration = 0.4f;
    public Ease flipEase = Ease.InOutSine;

    private SpriteRenderer sprRenderer;
    private Tween flipTween;
    private float scale;

    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        scale = transform.localScale.x;
        if (!facingRight)
            scale = -scale;
    }

    private void Update()
    {
        if (rb != null)
        {
            if (facingRight)
            {
                if(rb.velocity.x < -REPOS)
                {
                    facingRight = false;
                    Flip(-scale);
                }
            }
            else
            {
                if (rb.velocity.x > REPOS)
                {
                    facingRight = true;
                    Flip(scale);
                }
            }
        }
    }

    void Flip(float scale)
    {
        if (flipTween != null && flipTween.IsActive())
            flipTween.Kill();
        flipTween = transform.DOScaleX(scale, flipDuration).SetEase(flipEase);
    }
}
