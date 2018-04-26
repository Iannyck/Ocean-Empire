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

    [Header("Sky")]
    public SerializableColor SkyColorTop = new SerializableColor(7f / 255, 188f / 255f, 254 / 255f);
    public SerializableColor SkyColorCenter = new SerializableColor(16f / 255, 204f / 255f, 242f / 255f);
    public SerializableColor SkyColorBottom = new SerializableColor(1, 1, 1);

    public MapData(string name)
    {
        Name = name;
    }
}
