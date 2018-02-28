using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : ScriptableObject
{
    public virtual object NewInstanceData(Rigidbody2D rb, BrainBehaviour component)
    {
        return null;
    }
    public virtual void Tick(Rigidbody2D rb, float deltaTime, object perInstanceData)
    {

    }
    public virtual void DrawGizmos(Rigidbody2D rb, object perInstanceData)
    {

    }
    public virtual void DrawGizmosSelected(Rigidbody2D rb, object perInstanceData)
    {

    }

    protected static void DrawPathGizmos(Vector2 from, Vector2 destination)
    {
        Gizmos.color = new Color(1, 1, 1);
        Gizmos.DrawLine(from, destination);
    }
}
