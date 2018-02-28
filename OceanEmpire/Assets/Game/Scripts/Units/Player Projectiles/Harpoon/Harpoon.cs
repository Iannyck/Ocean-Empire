using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : Projectile
{
    public float killAfter = 5;
    private float deathTimer;
    public SpriteRenderer harpoonSpriteRenderer;

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
            harpoonBlocker.HarpoonHit();
            Kill();
        }
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