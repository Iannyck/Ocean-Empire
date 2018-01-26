using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Var_NewBool", menuName = "CCC/Variable/Bool")]
public class BoolVariable : VarVariable<bool>
{
    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(BoolVariable value)
    {
        Value = value.Value;
    }
}