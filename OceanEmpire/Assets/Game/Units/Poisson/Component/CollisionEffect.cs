using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : EffectComponent {

    public OnCollision collision;
    public OnTrigger triggerCollision;

    protected override void StartEffect()
    {
        SearchForCollisionScript();
    }

    void SearchForCollisionScript()
    {
        if (collision != null)
            collision.onEnter.AddListener(OnCollisionEnterEvent);

        if (triggerCollision != null)
            triggerCollision.onEnter.AddListener(OnTriggerEnterEvent);
    }

    protected virtual void OnCollisionEnterEvent(ColliderInfo info, Collision2D col) { }

    protected virtual void OnTriggerEnterEvent(ColliderInfo info, Collider2D col) { }
}
