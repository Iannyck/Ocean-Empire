using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratedSpriteKit : ISpriteKit, IGenerated
{
    public List<TriColoredSprite> triColoredSprites = new List<TriColoredSprite>();

    [SerializeField] private string generationCode;

    public GeneratedSpriteKit(string generationCode)
    {
        this.generationCode = generationCode;
    }

    /// <summary>
    /// NB: Réfère à la liste (ne la copie pas)
    /// </summary>
    public GeneratedSpriteKit(string generationCode, List<TriColoredSprite> triColoredSprites)
    {
        this.generationCode = generationCode;
        this.triColoredSprites = triColoredSprites;
    }

    /// <summary>
    /// NB: Copie la liste
    /// </summary>
    public GeneratedSpriteKit(string generationCode, IEnumerable<TriColoredSprite> triColoredSprites)
    {
        this.generationCode = generationCode;
        this.triColoredSprites = new List<TriColoredSprite>(triColoredSprites);
    }

    public int Length
    {
        get { return triColoredSprites.Count; }
    }

    public TriColoredSprite Get(int index)
    {
        return triColoredSprites[index];
    }

    public string GetGenerationCode()
    {
        return generationCode;
    }
}
