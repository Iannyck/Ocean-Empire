using UnityEngine;
using System;

[Serializable]
public class ColoredSpriteGradient
{
    public Sprite sprite;
    public Gradient gradient;

    public ColoredSprite Evaluate(float time)
    {
        return new ColoredSprite(sprite, gradient.Evaluate(time));
    }
}
