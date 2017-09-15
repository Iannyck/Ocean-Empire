using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigger : MonoBehaviour
{
    public class OnTriggerEvent : UnityEvent<Collider2D> { }

    public List<ColliderInfo.ParentType> filter = new List<ColliderInfo.ParentType>();
    public OnTriggerEvent onEnter;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ColliderInfo info = collider.GetComponent<ColliderInfo>();

        if (filter.Contains(info.parentType))
            onEnter.Invoke(collider);
    }
}
