using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneratedSpriteKit : ISpriteKit, IGenerated
{
    public List<Sprite> sprites = new List<Sprite>();

    private string generationCode;

    public GeneratedSpriteKit(string generationCode)
    {
        this.generationCode = generationCode;
    }
    public GeneratedSpriteKit(string generationCode, List<Sprite> sprites)
    {
        this.generationCode = generationCode;
        this.sprites = sprites;
    }
    public GeneratedSpriteKit(string generationCode, IEnumerable<Sprite> sprites)
    {
        this.generationCode = generationCode;
        this.sprites = new List<Sprite>(sprites);
    }

    public int Length
    {
        get { return sprites.Count; }
    }

    public void Get(int index, out Sprite sprite, out Color color)
    {
        throw new System.NotImplementedException();
    }

    public string GetGenerationCode()
    {
        return generationCode;
    }
}
