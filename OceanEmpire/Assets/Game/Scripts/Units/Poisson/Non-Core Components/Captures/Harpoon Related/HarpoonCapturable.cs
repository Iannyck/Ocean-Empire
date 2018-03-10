using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Capturable)), DisallowMultipleComponent]
public class HarpoonCapturable : HarpoonBlocker
{
    public Capturable Capturable { get { return GetComponent<Capturable>(); } }

    public Locker canBeHarpooned = new Locker();

    public override void HarpoonHit()
    {
        if (!canBeHarpooned)
            return;

        var capturable = GetComponent<Capturable>();
        if (capturable != null)
            Game.PlayerStats.TryCapture(capturable);
    }
}
