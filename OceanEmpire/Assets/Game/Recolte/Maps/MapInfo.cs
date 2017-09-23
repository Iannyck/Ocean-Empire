using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public const float MAP_WIDTH = 5;
    public const float MAP_RIGHT = MAP_WIDTH / 2;
    public const float MAP_LEFT = MAP_WIDTH / -2;

    /// <summary>
    /// À L'horizontal seulement
    /// </summary>
    public static bool IsOutOfBounds(Vector2 v) { return v.x > MAP_RIGHT || v.x < MAP_LEFT; }

    public string regionName = "YourRegionName";
    public float heightMax = 0;
    public float depthMax = 10000;
    public const float DEPTHSCALING = 100;
}
