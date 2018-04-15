using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC;
using UnityEditor;

public class Electrify : CollisionEffect
{
    [SerializeField] protected float repulsionStrength = 2.5f;
    [SerializeField] protected bool electrifiedOnStart = true;

    [Header("Animation"), SerializeField] protected Animator electricAnimator;
    [SerializeField] protected string animatorBoolName;
    [SerializeField] protected GameObject victimVFX;
    [SerializeField] protected float stunDuration = 1f;

    private bool _isElectrified = false;
    private const string LOCKCAPTURE_KEY = "elec";
    private MeleeCapturable _meleeCapturable;
    private int _electricId;

    protected override void Awake()
    {
        base.Awake();

        if (electricAnimator != null)
            _electricId = Animator.StringToHash(animatorBoolName);
    }

    protected virtual void OnEnable()
    {
        Electrified = electrifiedOnStart;
    }

    public virtual bool Electrified
    {
        get { return _isElectrified; }
        set
        {
            bool applyChanges = _isElectrified != value;
            _isElectrified = value;

            if (applyChanges)
                ApplyElectrify();
        }
    }

    protected virtual void ApplyElectrify()
    {
        if (Electrified)
        {
            // Disable melee capture
            _meleeCapturable = GetComponent<MeleeCapturable>();
            if (_meleeCapturable != null)
                _meleeCapturable.canCapture.Lock(LOCKCAPTURE_KEY);
        }
        else
        {
            // Re-enable melee capture
            if (_meleeCapturable != null)
                _meleeCapturable.canCapture.Unlock(LOCKCAPTURE_KEY);
        }

        if (electricAnimator != null)
            electricAnimator.SetBool(_electricId, Electrified);
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
        if (subarmineBump != null)
        {
            subarmineBump.Bump(repulsionVector, stunDuration);
        }
        else
        {
            target.AddForce(repulsionVector, ForceMode2D.Impulse);
        }

        if (victimVFX != null)
        {
            victimVFX.Duplicate(target.transform).AddComponent<SelfDestruct>().delay = stunDuration;
        }
    }
}
