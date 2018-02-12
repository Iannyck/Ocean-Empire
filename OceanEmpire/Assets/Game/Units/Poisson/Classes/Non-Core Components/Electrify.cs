using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC;

public class Electrify : CollisionEffect
{
    public float electricInterval = 2.5f;
    public float electricDuration = 2.5f;

    public float repulsionStrength = 2.5f;

    // Animation temporaire
    public SpriteRenderer fishSprite; 
    private Color originColor;

    private bool isElectrified = false;


    private MeleeCapture captureEffect;

    protected override void StartEffect()
    {
        base.StartEffect();

        captureEffect = GetComponent<MeleeCapture>();

        originColor = fishSprite.color;

        ElectricEffect();
    }

    void ElectricEffect()
    {
        isElectrified = true;
        
        if (captureEffect != null)
            captureEffect.canCapture = false;

        ElectricAnimation();

        this.DelayedCall(delegate () {
            StopElectricEffect();
            this.DelayedCall(ElectricEffect, electricInterval);
        }, electricDuration);
    }

    void StopElectricEffect()
    {
        isElectrified = false;

        if (captureEffect != null)
            captureEffect.canCapture = true;

        StopElectricAnimation();
    }

    void ElectricAnimation()
    {
        fishSprite.color = Color.yellow;
    }

    void StopElectricAnimation()
    {
        fishSprite.color = originColor;
    }

    protected override void OnCollisionEnterEvent(ColliderInfo info, Collision2D col)
    {
        base.OnCollisionEnterEvent(info, col);
        Repulse(col.rigidbody, col.otherRigidbody.position);
    }

    protected override void OnTriggerEnterEvent(ColliderInfo info, Collider2D col)
    {
        base.OnTriggerEnterEvent(info, col);
        Repulse(col.attachedRigidbody, transform.position);
    }

    void Repulse(Rigidbody2D target, Vector2 myPosition)
    {
        if (isElectrified)
        {
            var repulsionDirection = (target.position - myPosition).normalized;
            target.AddForce(repulsionDirection * repulsionStrength, ForceMode2D.Impulse);
        }
    }
}
