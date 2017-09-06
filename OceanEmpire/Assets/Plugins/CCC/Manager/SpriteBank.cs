using UnityEngine;

[CreateAssetMenu(menuName = "CCC/SpriteBank")]
public class SpriteBank : ScriptableObject
{
    static private SpriteBank instance;
    public Sprite[] sprites = new Sprite[0];

    public void SetAsInstance()
    {
        instance = this;
    }

    public int GetIndex_i(Sprite sprite)
    {
        for (int i = 0; i < sprites.Length; i++)
            if (sprites[i] == sprite)
                return i;
        throw new System.Exception("Sprite not in bank");
    }

    public bool IsInBank(Sprite sprite)
    {
        for (int i = 0; i < sprites.Length; i++)
            if (sprites[i] == sprite)
                return true;
        return false;
    }

    static public Sprite GetSprite(int index)
    {
        if (instance == null)
        {
            if (Application.isPlaying)
            {
                Debug.LogError("There is no SpriteBank");
                return null;
            }
            else
            {
                SpriteBank assetInstance = Resources.Load<SpriteBank>("SpriteBank");
                if (assetInstance != null)
                {
                    return assetInstance.sprites[index];
                }
                else
                {
                    throw new System.Exception("Cannot get sprite index. No SpriteBank named 'SpriteBank' in the Resources folder.");
                }
            }
        }
        else
        {
            if (instance.sprites == null || instance.sprites.Length <= index || index < 0)
            {
                Debug.LogError("No sprite to index " + index + " in the SpriteBank");
                return null;
            }

            return instance.sprites[index];
        }
    }

    static public int GetIndex(Sprite sprite)
    {
        if (instance == null)
        {
            SpriteBank assetInstance = Resources.Load<SpriteBank>("SpriteBank");
            if(assetInstance != null)
            {
                return assetInstance.GetIndex_i(sprite);
            }
            else
            {
                throw new System.Exception("Cannot get sprite index. No SpriteBank named 'SpriteBank' in the Resources folder.");
            }
        }
        else
             return instance.GetIndex_i(sprite);
    }
}
