using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FloatReference : VarReference<FloatVariable, float>
{
    public FloatReference() : base()
    { }

    public FloatReference(float value) : base(value)
    { }
}