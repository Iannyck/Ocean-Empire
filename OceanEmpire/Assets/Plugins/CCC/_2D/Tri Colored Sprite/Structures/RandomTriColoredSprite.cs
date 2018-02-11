using UnityEngine;
using System;

[Serializable]
public class RandomTriColoredSprite
{
    public Sprite sprite;
    public RandomHSVColor rColor = new RandomHSVColor(Color.red, Color.red);
    public RandomHSVColor gColor = new RandomHSVColor(Color.green, Color.green);
    public RandomHSVColor bColor = new RandomHSVColor(Color.blue, Color.blue);

    public TriColoredSprite GetRandomTriColoredSprite()
    {
        return new TriColoredSprite(sprite,
            rColor.GetRandomColor(),
            gColor.GetRandomColor(),
            bColor.GetRandomColor());
    }
}
