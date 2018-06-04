using UnityEngine;
using System;

[Serializable]
public class TriColoredSprite
{
    public Sprite sprite;
    public Color colorR = Color.red;
    public Color colorG = Color.green;
    public Color colorB = Color.blue;

    public TriColoredSprite(Sprite sprite, Color colorR, Color colorG, Color colorB)
    {
        this.sprite = sprite;
        this.colorB = colorB;
        this.colorG = colorG;
        this.colorR = colorR;
    }
}