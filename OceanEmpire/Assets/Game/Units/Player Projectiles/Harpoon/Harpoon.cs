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

    public void HitBigFish(ColliderInfo info, Collider2D collider)
    {
        Capturable capturable = info.Parent.GetComponent<Capturable>();

        if (capturable == null)
        {
            Debug.LogError("Le harpoon a frappé un objet qui n'est pas un big fish.");
            return;
        }

        //bigFish.Capture();
        Game.PlayerStats.TryCapture(capturable);
        Kill();
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}