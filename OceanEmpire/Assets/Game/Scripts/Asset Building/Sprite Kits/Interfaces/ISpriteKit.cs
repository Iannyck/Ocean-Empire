using UnityEngine;

public interface ISpriteKit
{
    void Get(int index, out TriColoredSprite triColoredSprite);

    int Length
    {
        get;
    }
}
