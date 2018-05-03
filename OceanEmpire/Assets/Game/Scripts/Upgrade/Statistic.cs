using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Statistic
{
    public string name;
    public float value;
    public string suffix;
    public bool hideIfZero;

    public Statistic(string name, float value, string suffix = "", bool hideIfZero = false)
    {
        this.name = name;
        this.value = value;
        this.suffix = suffix;
        this.hideIfZero = hideIfZero;
    }
}