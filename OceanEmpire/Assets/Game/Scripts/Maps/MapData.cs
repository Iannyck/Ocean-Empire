using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public string Name;

    [ReadOnly]
    public string GameSceneName;

    [Header("Ocean Settings")]
    public float Depth;
    public SerializableColor ShallowColor;
    public SerializableColor DeepColor;

    public MapData(string name)
    {
        Name = name;
    }
}
