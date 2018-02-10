using UnityEngine;

public interface ISpriteKit
{
    void Get(int index, out Sprite sprite, out Color color);

    int Length
    {
        get;
    }
}
