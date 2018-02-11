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
}
