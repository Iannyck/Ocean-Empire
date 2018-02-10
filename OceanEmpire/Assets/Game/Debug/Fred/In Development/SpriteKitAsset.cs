using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Sprites/Sprite Kit Asset")]
public class SpriteKitAsset : ScriptableObject, ISpriteKit
{

    [SerializeField] Sprite[] sprites = new Sprite[1];

    public int Length
    {
        get { return sprites.Length; }
    }

    public void Get(int index, out Sprite sprite, out Color color)
    {
        throw new System.NotImplementedException();
    }
}
