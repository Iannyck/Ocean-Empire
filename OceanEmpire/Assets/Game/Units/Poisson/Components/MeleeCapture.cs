using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCapture : CollisionEffect
{
    private MiniFish fish;

    [HideInInspector]
    public bool canCapture = true;

    protected override void StartEffect()
    {
        base.StartEffect();

        canCapture = true;

        fish = GetComponent<MiniFish>();
        if (fish == null)
            Debug.Log("Error, no fish script");
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
        if (canCapture)
            fish.OnPlayerTouch();
    }
}
