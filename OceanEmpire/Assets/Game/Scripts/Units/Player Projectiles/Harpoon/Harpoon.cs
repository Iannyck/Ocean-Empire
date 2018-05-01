using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : Projectile
{
    public float killAfter = 5;
    private float deathTimer;
    public SpriteRenderer harpoonSpriteRenderer;
    public TriColored triColored;
    public new Collider2D collider;

    protected override void Start()
    {
        base.Start();
        deathTimer = killAfter;
    }

    private void Update()
    {
        if (deathTimer > 0)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                Kill();
            }
        }
    }

    public void OnUnitHit(ColliderInfo info, Collider2D collider)
    {
        var harpoonBlocker = info.Parent.GetComponent<HarpoonBlocker>();

        if (harpoonBlocker != null)
        {
            bool harpoonRepelled;
            harpoonBlocker.HarpoonHit(out harpoonRepelled);
            if (harpoonRepelled)
                BounceOffThenKill(info.transform.position);
            else
                Kill();
        }
    }

    protected void BounceOffThenKill(Vector2 bounceOffPosition)
    {
        collider.enabled = false;
        rb.velocity = rb.velocity * -0.5f;
        rb.drag = 3;

        Color r = triColored.ColorR;
        Color g = triColored.ColorG;
        Color b = triColored.ColorB;
        Color rd = r.ChangedAlpha(0);
        Color gd = g.ChangedAlpha(0);
        Color bd = b.ChangedAlpha(0);

        float fade = 0;
        DOTween.To(() => fade, (x) =>
          {
              fade = x;
              triColored.SetColors(
                  Color.Lerp(r, rd, fade),
                  Color.Lerp(g, gd, fade),
                  Color.Lerp(b, bd, fade));
          }, 1, 0.75f).SetDelay(0.5f).onComplete = Kill;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }

    void SetHarpoonSprite(Sprite hSprite)
    {
        harpoonSpriteRenderer.sprite = hSprite;
    }
}