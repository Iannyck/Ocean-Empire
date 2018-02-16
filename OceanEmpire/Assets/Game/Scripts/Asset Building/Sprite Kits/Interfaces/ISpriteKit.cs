using UnityEngine;

public interface ISpriteKit
{
    TriColoredSprite Get(int index);

    int Length
    {
        get;
    }
}
