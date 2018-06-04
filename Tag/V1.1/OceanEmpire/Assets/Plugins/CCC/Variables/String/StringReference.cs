using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringReference : VarReference<StringVariable, string>
{
    public StringReference() : base()
    { }

    public StringReference(string value) : base(value)
    {
    }
}
