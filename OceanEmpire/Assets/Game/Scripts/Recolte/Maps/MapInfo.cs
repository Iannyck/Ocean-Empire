using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    //[InspectorCategory("General")]
    public Transform PlayerSpawn;
    //[InspectorCategory("General")]
    public Transform PlayerStart_;

    public const float MAP_WIDTH = 5;
    public const float MAP_RIGHT = MAP_WIDTH / 2;
    public const float MAP_LEFT = MAP_WIDTH / -2;

    /// <summary>
    /// À L'horizontal seulement
    /// </summary>
    public static bool IsOutOfHorizontalBounds(Vector2 v) { return v.x > MAP_RIGHT || v.x < MAP_LEFT; }

    [Header("General")]
    public float mapTop = 0;
    [Header("General")]
    public float mapBottom = -100;

    public const float DEPTHSCALING = 100;

    public float MapHeight
    {
        get
        {
            return mapTop - mapBottom;
        }
    }

    /// <summary>
    /// 0 = départ (surface)
    /// <para/>
    /// 1 = arrivé (fond de l'océan)
    /// </summary>
    public float GetMapPosition01(float height)
    {
        return (mapTop - height) / MapHeight;
    }

    /// <summary>
    /// 0 = départ (surface)
    /// <para/>
    /// 1 = arrivé (fond de l'océan)
    /// </summary>
    public float GetMapHeightFromPosition01(float pos01)
    {
        return mapTop - (MapHeight * pos01);
    }
}
