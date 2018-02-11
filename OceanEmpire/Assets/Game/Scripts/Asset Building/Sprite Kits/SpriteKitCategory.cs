using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName ="SK_NewCategory", menuName = "Ocean Empire/Sprite Kit/Category")]
public class SpriteKitCategory : ScriptableObject
{
    public List<RandomTriColoredSprite> elements = new List<RandomTriColoredSprite>();


    public void PickAndAppend(StringBuilder genCodeBuilder, List<TriColoredSprite> result)
    {
        if (elements.Count <= 0)
        {
            genCodeBuilder.Append(' ');
            result.Add(null);
            Debug.LogError("Error in kit generator (" + name + "): Empty list");
            return;
        }

        var index = Random.Range(0, elements.Count);
        var pick = elements[index].GetRandomTriColoredSprite();
        result.Add(pick);

        genCodeBuilder.Append(index);
        genCodeBuilder.Append('#');
        genCodeBuilder.Append(ColorUtility.ToHtmlStringRGBA(pick.colorR));
        genCodeBuilder.Append('#');
        genCodeBuilder.Append(ColorUtility.ToHtmlStringRGBA(pick.colorG));
        genCodeBuilder.Append('#');
        genCodeBuilder.Append(ColorUtility.ToHtmlStringRGBA(pick.colorB));
    }

    public bool ReadFrom(string subCode, List<TriColoredSprite> result)
    {
        if (subCode == " ")
        {
            result.Add(null);
            Debug.LogError("Error in kit category (" + name + "): Null in genCode");
            return false;
        }

        int left = 0;
        int right = subCode.IndexOf('#');
        int index;
        bool success = int.TryParse(subCode.Substring(0, right), out index);

        if (index >= elements.Count)
        {
            index = elements.Count - 1;
            success = false;
        }

        Color rColor;
        success &= ParseColorFrom(ref left, ref right, '#', ref subCode, out rColor);
        Color gColor;
        success &= ParseColorFrom(ref left, ref right, '#', ref subCode, out gColor);
        Color bColor;
        success &= ParseColorFrom(ref left, ref right, '\0', ref subCode, out bColor);

        result.Add(new TriColoredSprite(elements[index].sprite, rColor, gColor, bColor));
        return success;
    }

    private bool ParseColorFrom(ref int left, ref int right, char startingChar, ref string code, out Color result)
    {
        left = right;
        right++;
        right = code.IndexOf(startingChar, right);
        if (right == -1)
            right = code.Length;
        string subString = code.Substring(left, right - left);
        return ColorUtility.TryParseHtmlString(subString, out result);
    }
}
