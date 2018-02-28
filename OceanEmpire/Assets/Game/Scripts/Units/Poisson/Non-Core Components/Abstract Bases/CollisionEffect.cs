using UnityEngine;

public abstract class CollisionEffect : MonoBehaviour
{
    [SerializeField] OnCollision collision;
    [SerializeField] OnTrigger triggerCollision;

    protected virtual void Awake()
    {
        // Add listeners
        if (collision != null)
            collision.onEnter.AddListener(OnCollisionEnterEvent);

        if (triggerCollision != null)
            triggerCollision.onEnter.AddListener(OnTriggerEnterEvent);
    }

    protected virtual void OnCollisionEnterEvent(ColliderInfo info, Collision2D col) { }

    protected virtual void OnTriggerEnterEvent(ColliderInfo info, Collider2D col) { }
}
