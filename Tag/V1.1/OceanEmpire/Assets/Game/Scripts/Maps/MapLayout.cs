using System.Collections.Generic;
using UnityEngine;

public class MapLayout : MonoBehaviour
{
    public Transform PlayerSpawn;
    public Transform PlayerStart_;

    public float BorderRight { get { return Width / 2; } }
    public float BorderLeft { get { return Width / -2; } }
    public float BorderTop { get { return 0; } }
    public float BorderBottom { get { return -Depth; } }
    public float Width { get { return GameCamera.CameraWidth; } }

    [SerializeField] float _depth = 150;
    public float Depth { get { return _depth; } private set { _depth = value; } }

    public const float UNIT_TO_METERS = 100;

    void Start()
    {
        Depth = _depth;
    }

    public void ApplyMapData(MapData mapData)
    {
        Depth = mapData.Depth;
    }

    /// <summary>
    /// 0 = départ (surface)
    /// <para/>
    /// 1 = arrivé (fond de l'océan)
    /// </summary>
    public float GetMapPosition01(float height)
    {
        return (BorderTop - height) / Depth;
    }

    /// <summary>
    /// 0 = départ (surface)
    /// <para/>
    /// 1 = arrivé (fond de l'océan)
    /// </summary>
    public float GetMapHeightFromPosition01(float pos01)
    {
        return BorderTop - (Depth * pos01);
    }
}
