using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public string Name;

    [ReadOnly]
    public string GameSceneName;

    public bool OverrideDepth = false;
    [ShowIf("OverrideDepth")]
    public float Depth;

    public bool OverrideColors = false;
    [ShowIf("OverrideColors")]
    public Color ShallowColor;
    [ShowIf("OverrideColors")]
    public Color DeepColor;

    public MapData(string name)
    {
        Name = name;
    }
}
