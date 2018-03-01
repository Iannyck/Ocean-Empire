using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC;

public class Electrify : CollisionEffect
{
    public float repulsionStrength = 2.5f;
    public bool electrifiedOnStart = true;
    // Animation temporaire
    public SpriteRenderer fishSprite;


    private Color originColor;
    private bool isElectrified = false;
    private const string LOCKCAPTURE_KEY = "elec";
    private MeleeCapturable meleeCapturable;

    protected override void Awake()
    {
        base.Awake();
        originColor = fishSprite.color;
    }

    protected virtual void OnEnable()
    {
        Electrified = electrifiedOnStart;
    }

    public virtual bool Electrified
    {
        get { return isElectrified; }
        set
        {
            bool applyChanges = isElectrified != value;
            isElectrified = value;

            if (applyChanges)
                ApplyElectrify();
        }
    }

    protected virtual void ApplyElectrify()
    {
        if (Electrified)
        {
            // Disable melee capture
            meleeCapturable = GetComponent<MeleeCapturable>();
            if (meleeCapturable != null)
                meleeCapturable.canCapture.Lock(LOCKCAPTURE_KEY);

            // Color change
            fishSprite.color = Color.blue;
        }
        else
        {
            // Re-enable melee capture
            if (meleeCapturable != null)
                meleeCapturable.canCapture.Unlock(LOCKCAPTURE_KEY);

            // Color change
            fishSprite.color = originColor;
        }
    }

    protected override void OnCollisionEnterEvent(ColliderInfo info, Collision2D col)
    {
        base.OnCollisionEnterEvent(info, col);

        if (Electrified)
            Repulse(col.rigidbody, col.otherRigidbody.position);
    }

    protected override void OnTriggerEnterEvent(ColliderInfo info, Collider2D col)
    {
        base.OnTriggerEnterEvent(info, col);

        if (Electrified)
            Repulse(col.attachedRigidbody, transform.position);
    }

    protected void Repulse(Rigidbody2D target, Vector2 myPosition)
    {
        var repulsionVector = (target.position - myPosition).normalized * repulsionStrength;
        var subarmineBump = target.GetComponent<SubmarineBump>();
        if(subarmineBump != null)
        {
            subarmineBump.Bump(repulsionVector);
        }
        else
        {
            target.AddForce(repulsionVector, ForceMode2D.Impulse);
        }
    }
}
