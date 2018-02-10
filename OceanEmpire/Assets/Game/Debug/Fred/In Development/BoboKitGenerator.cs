using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Experimental/Bobo Kit Generator")]
public class BoboKitGenerator : ScriptableObject, ISpriteKitGenerator
{
    [SerializeField] List<ColoredSpriteGradient> faces = new List<ColoredSpriteGradient>();
    [SerializeField] List<ColoredSpriteGradient> bodies = new List<ColoredSpriteGradient>();
    [SerializeField] List<ColoredSpriteGradient> tails = new List<ColoredSpriteGradient>();

    public GeneratedSpriteKit GenerateSpriteKit(string generationCode)
    {
        return null;
    }
}
