using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpriteKitGenerator
{
    GeneratedSpriteKit GenerateSpriteKit(string generationCode);
}
