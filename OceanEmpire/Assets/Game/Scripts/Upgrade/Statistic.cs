using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Statistic
{
    public string name;
    public float value;

    public Statistic(string n, float v)
    {
        name = n;
        value = v;
    }
}