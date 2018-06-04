using UnityEngine;

public class ColoredAttribute : PropertyAttribute
{
    public readonly Color color;

    public ColoredAttribute(float r, float g, float b)
    {
        color = new Color(r, g, b);
    }
}
