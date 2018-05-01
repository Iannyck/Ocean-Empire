using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Statistic
{
    public string name;
    public float value;
    public string suffix;

    public Statistic(string name, float value, string suffix = "")
    {
        this.name = name;
        this.value = value;
        this.suffix = suffix;
    }
}