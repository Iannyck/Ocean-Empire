using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomHSVColor
{
    public Color left = Color.white;
    public Color right = Color.white;

    public RandomHSVColor(Color leftColor, Color rightColor)
    {
        left = leftColor;
        right = rightColor;
    }

    public Color GetRandomColor()
    {
        Vector2 h = new Vector2();
        Vector2 s = new Vector2();
        Vector2 v = new Vector2();

        Color.RGBToHSV(left, out h.x, out s.x, out v.x);
        Color.RGBToHSV(right, out h.y, out s.y, out v.y);

        if (s.x > s.y)
            s = s.SwapXAndY();

        if (v.x > v.y)
            v = v.SwapXAndY();

        if (h.x > h.y)
        {
            var delta = 1 - (h.x - h.y);
            if (Random.Range(0, delta) > h.y)
                h.y = 1;
            else
                h.x = 0;
        }

        return Random.ColorHSV(h.x, h.y, s.x, s.y, v.x, v.y);
    }
}
