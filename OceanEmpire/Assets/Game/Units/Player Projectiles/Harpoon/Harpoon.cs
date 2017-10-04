using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpoon : Projectile
{
    public void HitBigFish(ColliderInfo info, Collider2D collider)
    {
        BigFish bigFish = info.Parent.GetComponent<BigFish>();
        if(bigFish == null)
        {
            "Le harpoon a frappé un objet qui n'est pas un big fish.".LogError();
            return;
        }

        bigFish.Capture();
    }
}