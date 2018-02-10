using UnityEngine;
using System;

[Serializable]
public struct ColoredSprite
{
    public Sprite sprite;
    public Color color;

    public ColoredSprite(Sprite sprite, Color color)
    {
        this.sprite = sprite;
        this.color = color;
    }
}
