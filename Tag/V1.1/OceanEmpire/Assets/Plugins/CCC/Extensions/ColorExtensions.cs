using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions
{
    public static Color ChangedAlpha(this Color c, float alpha)
    {
        return new Color(c.r, c.g, c.b, alpha);
    }
}
