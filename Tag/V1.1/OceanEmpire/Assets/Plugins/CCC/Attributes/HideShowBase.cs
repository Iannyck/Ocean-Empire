using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HideShowBaseAttribute : PropertyAttribute
{
    public readonly string name;
    public readonly Type type;
    public enum Type { Field, Property, Method }

    public HideShowBaseAttribute(string name, Type type)
    {
        this.name = name;
        this.type = type;
    }
}
