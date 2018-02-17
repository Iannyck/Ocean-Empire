using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SK_Prebuilt_NewKit", menuName = "Ocean Empire/Sprite Kit/Prebuilt")]
public class PrebuiltSpriteKit : ScriptableObject, ISpriteKit
{

    [SerializeField] TriColoredSprite[] triColoredSprites = new TriColoredSprite[1];

    public int Length
    {
        get { return triColoredSprites.Length; }
    }

    public TriColoredSprite Get(int index)
    {
        return triColoredSprites[index];
    }
}
