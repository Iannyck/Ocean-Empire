using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SerializableColor
{
    public float r;
    public float g;
    public float b;
    public float a;

    public SerializableColor(Color color) : this(color.r, color.g, color.b, color.a) { }
    public SerializableColor(float r, float g, float b) : this(r, g, b, 1) { }
    public SerializableColor(float r, float g, float b, float a)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    public Color ToColor()
    {
        return new Color(r, g, b, a);
    }

    public static implicit operator Color(SerializableColor c)
    {
        return c.ToColor();
    }
    public override int GetHashCode()
    {
        return ToColor().GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is SerializableColor))
            return false;

        var otherCol = (SerializableColor)obj;

        return otherCol.r == r
            && otherCol.g == g
            && otherCol.b == b
            && otherCol.a == a;
    }
    public override string ToString()
    {
        return ToColor().ToString();
    }
}
