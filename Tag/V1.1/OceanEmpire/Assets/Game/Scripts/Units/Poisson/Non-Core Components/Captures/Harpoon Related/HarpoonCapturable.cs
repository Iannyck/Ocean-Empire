using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Capturable)), DisallowMultipleComponent]
public class HarpoonCapturable : HarpoonBlocker
{
    public Capturable Capturable { get { return GetComponent<Capturable>(); } }

    public Locker canBeHarpooned = new Locker();

    public override void HarpoonHit(out bool repelHarpoon)
    {
        if (!canBeHarpooned)
        {
            repelHarpoon = true;
            return;
        }

        repelHarpoon = false;

        var capturable = GetComponent<Capturable>();
        if (capturable != null)
            Game.Instance.PlayerStats.TryCapture(capturable, CaptureTechnique.Harpoon);
    }
}
