using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    [System.Serializable]
    public class OnTriggerEvent : UnityEvent<ColliderInfo, Collider2D> { }

    [BitMask(typeof(ColliderInfo.ParentType))]
    public ColliderInfo.ParentType filter;

    public OnTriggerEvent onEnter;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ColliderInfo info = collider.GetComponent<ColliderInfo>();
        if (info == null)
            return;

        if ((filter & info.parentType) != 0)
            onEnter.Invoke(info, collider);
    }
}
