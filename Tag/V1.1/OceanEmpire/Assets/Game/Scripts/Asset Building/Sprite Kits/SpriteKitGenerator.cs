using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "SK_Gen_NewGenerator", menuName = "Ocean Empire/Sprite Kit/Generator")]
public class SpriteKitGenerator : ScriptableObject
{
    private const char SPLIT = '/';

    [Header("MUST REMAIN IN ORDER"), SerializeField, Reorderable]
    private List<SpriteKitCategory> categories = new List<SpriteKitCategory>();

    public virtual GeneratedSpriteKit GenerateNewSpriteKit()
    {
        List<TriColoredSprite> result = new List<TriColoredSprite>(3);

        StringBuilder genCodeBuilder = new StringBuilder();

        for (int i = 0; i < categories.Count; i++)
        {
            categories[i].PickAndAppend(genCodeBuilder, result);
            if (i < categories.Count - 1)
                genCodeBuilder.Append(SPLIT);
        }

        return new GeneratedSpriteKit(genCodeBuilder.ToString(), result);
    }

    public virtual GeneratedSpriteKit GenerateSpriteKit(string generationCode)
    {
        List<TriColoredSprite> result = new List<TriColoredSprite>(3);
        string[] subCodes = generationCode.Split(SPLIT);

        bool success = true;
        try
        {
            for (int i = 0; i < categories.Count; i++)
            {
                success &= categories[i].ReadFrom(subCodes[i], result);
            }
        }
        catch
        {
            success = false;
        }

        if (!success)
            Debug.LogError("Failed to correctly rebuild SpriteKit (" + name + ')');

        return new GeneratedSpriteKit(generationCode, result);
    }
}
