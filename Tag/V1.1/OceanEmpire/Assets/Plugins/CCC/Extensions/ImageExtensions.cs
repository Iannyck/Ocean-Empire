using UnityEngine.UI;
using UnityEngine;

public static class ImageExtensions
{
    public static void SetAlpha(this Image i, float alpha)
    {
        Color c = i.color;
        i.color = new Color(c.r, c.g, c.b, alpha);
    }
}
