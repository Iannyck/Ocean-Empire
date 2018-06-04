using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class VarReference<A, V> where A : VarVariable<V>
{
    public bool UseConstant = true;
    public V ConstantValue;
    public A Variable;

    public VarReference()
    { }

    public VarReference(V value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public V Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set
        {
            if (UseConstant)
                ConstantValue = value;
            else
                Variable.Value = value;
        }
    }

    public static implicit operator V(VarReference<A, V> reference)
    {
        return reference.Value;
    }
}
