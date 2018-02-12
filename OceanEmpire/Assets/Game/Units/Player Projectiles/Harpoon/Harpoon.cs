using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : Projectile
{
    public float killAfter = 5;

    private float deathTimer;
    private bool isDead = false;

    private void Update()
    {
        if(deathTimer > 0)
        {
            deathTimer -= killAfter;
            if(deathTimer <= 0)
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

    void Kill()
    {
        Destroy(gameObject);
    }
}