using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
    [System.Serializable]
    public class OnCollisionEvent : UnityEvent<ColliderInfo, Collision2D> { }

    [BitMask(typeof(ColliderInfo.ParentType))]
    public ColliderInfo.ParentType filter;

    public OnCollisionEvent onEnter;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderInfo info = collision.gameObject.GetComponent<ColliderInfo>();

        if ((filter & info.parentType) != 0)
            onEnter.Invoke(info, collision);
    }
}
