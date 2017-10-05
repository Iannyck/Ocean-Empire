using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : Projectile, IKillable
{
    public float killAfter = 5;

    private float deathTimer;
    private bool isDead = false;

    protected override void Awake()
    {
        base.Awake();

        Destroy(gameObject, killAfter);
    }

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
        BigFish bigFish = info.Parent.GetComponent<BigFish>();

        if (bigFish == null)
        {
            Debug.LogError("Le harpoon a frappé un objet qui n'est pas un big fish.");
            return;
        }

        bigFish.Capture();
        Kill();
    }

    public void Kill()
    {
        if (isDead)
            return;

        isDead = true;

        //A remplacer lors de l'implemention du pooling system
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}