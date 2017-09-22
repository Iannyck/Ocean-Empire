using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    [System.Serializable]
    public class OnCollisionEvent : UnityEvent<Collision2D> { }

    public List<ColliderInfo.ParentType> filter = new List<ColliderInfo.ParentType>();
    public OnCollisionEvent onEnter;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderInfo info = collision.gameObject.GetComponent<ColliderInfo>();

        if (filter.Contains(info.parentType))
            onEnter.Invoke(collision);
    }
}
