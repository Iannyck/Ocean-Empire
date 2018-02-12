using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Capturable))]
public class MeleeCapture : CollisionEffect
{
    Capturable capturable;

    [HideInInspector]
    public bool canCapture = true;

    protected override void Awake()
    {
        capturable = GetComponent<Capturable>();
        base.Awake();
    }

    protected override void StartEffect()
    {
        base.StartEffect();

        canCapture = true;
    }

    protected override void OnCollisionEnterEvent(ColliderInfo info, Collision2D col)
    {
        base.OnCollisionEnterEvent(info, col);
        Capture();
    }

    protected override void OnTriggerEnterEvent(ColliderInfo info, Collider2D col)
    {
        base.OnTriggerEnterEvent(info, col);
        Capture();
    }

    void Capture()
    {
        // NB: LA LIGNE SUIVANT EST TEMPORAIRE. Ça fait beaucoup trop weird de passé par PlayerStats pour capturer quelque-chose
        if (canCapture)
            Game.PlayerStats.TryCapture(capturable);
    }
}
