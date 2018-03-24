using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public Transform PlayerSpawn;
    public Transform PlayerStart_;

    public static float BorderRight { get { return Width / 2; } }
    public static float BorderLeft { get { return Width / -2; } }
    public static float Width { get { return GameCamera.CameraWidth; } }

    /// <summary>
    /// À L'horizontal seulement
    /// </summary>
    public static bool IsOutOfHorizontalBounds(Vector2 v) { return v.x > BorderRight || v.x < BorderLeft; }

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
