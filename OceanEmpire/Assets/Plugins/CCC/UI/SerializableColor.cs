using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableColor
{
    public float r = 0;
    public float g = 0;
    public float b = 0;
    public float a = 0;

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
}
