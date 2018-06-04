using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "SK_NewCategory", menuName = "Ocean Empire/Sprite Kit/Category")]
public class SpriteKitCategory : ScriptableObject
{
    [SerializeField] private List<RandomTriColoredSprite> elements = new List<RandomTriColoredSprite>();

    [SerializeField] private List<bool> actives = new List<bool>();
    [SerializeField] private int solo = -1;


    public ReadOnlyCollection<RandomTriColoredSprite> GetElements() { return elements.AsReadOnly(); }
    public ReadOnlyCollection<bool> GetActives() { return actives.AsReadOnly(); }
    public int GetSoloElement() { return solo; }
    public void SetSoloElement(int index) { solo = index; }
    public bool IsSoloEnabled { get { return solo != -1; } }
    public void DisableSoloElement() { solo = -1; }

    private bool VerifyIndex(int index)
    {
        if (index < 0 || index >= elements.Count)
        {
            Debug.LogError("Invalid index");
            return false;
        }
        return true;
    }

    public void RemoveElement(int index)
    {
        if (VerifyIndex(index))
        {
            elements.RemoveAt(index);
            actives.RemoveAt(index);
            if (solo == index)
                solo = -1;
        }
    }
    public void AddElement(RandomTriColoredSprite element, bool isActive = true)
    {
        elements.Add(element);
        actives.Add(isActive);
    }

    public void ReorderElements(int a, int b)
    {
        var wasA = elements[a];
        elements[a] = elements[b];
        elements[b] = wasA;

        var wasActiveA = actives[a];
        actives[a] = actives[b];
        actives[b] = wasActiveA;

        if (solo == a)
            solo = b;
        else if (solo == b)
            solo = a;
    }


    public void SetElementActive(int index, bool active)
    {
        actives[index] = active;
    }

    public void Clear()
    {
        actives.Clear();
        elements.Clear();
    }

    public bool VerifyIntegrity()
    {
        return actives.Count == elements.Count && solo < actives.Count;
    }

    public TriColoredSprite Pick()
    {
        if (solo != -1)
            return elements[solo].GetRandomTriColoredSprite();

        RandomTriColoredSprite[] pickList = new RandomTriColoredSprite[elements.Count];
        int u = 0;
        for (int i = 0; i < elements.Count; i++)
        {
            if (actives[i])
            {
                pickList[u] = elements[i];
                u++;
            }
        }
        if (u == 0)
            return null;

        RandomTriColoredSprite pick = null;
        if (u == 1)
            pick = pickList[0];
        else
            pick = pickList[Random.Range(0, u)];
        return pick == null ? null : pick.GetRandomTriColoredSprite();
    }

    public TriColoredSprite Pick(out int index)
    {
        if (solo != -1)
        {
            index = solo;
            return elements[solo].GetRandomTriColoredSprite();
        }

        RandomTriColoredSprite[] pickList = new RandomTriColoredSprite[elements.Count];
        int u = 0;
        for (int i = 0; i < elements.Count; i++)
        {
            if (actives[i])
            {
                pickList[u] = elements[i];
                u++;
            }
        }
        if (u == 0)
        {
            Debug.LogError("Error, no active element");
            index = -1;
            return null;
        }

        RandomTriColoredSprite pick = null;
        if (u == 1)
            pick = pickList[0];
        else
            pick = pickList[Random.Range(0, u)];

        index = elements.IndexOf(pick);
        return pick.GetRandomTriColoredSprite();
    }


    public void PickAndAppend(StringBuilder genCodeBuilder, List<TriColoredSprite> result)
    {
        int index;
        var pick = Pick(out index);
        result.Add(pick);

        if(pick == null)
        {
            genCodeBuilder.Append(' ');
            Debug.LogError("Error in kit generator (" + name + "): Empty list");
            return;
        }

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
