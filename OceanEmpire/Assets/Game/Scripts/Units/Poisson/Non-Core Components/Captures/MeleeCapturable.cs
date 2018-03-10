using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Capturable))]
public class MeleeCapturable : CollisionEffect
{
    [HideInInspector]
    public Locker canCapture = new Locker();

    public Capturable Capturable { get { return GetComponent<Capturable>(); } }

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
        var capturable = GetComponent<Capturable>();
        if(capturable != null && canCapture)
            Game.PlayerStats.TryCapture(capturable);
    }
}
