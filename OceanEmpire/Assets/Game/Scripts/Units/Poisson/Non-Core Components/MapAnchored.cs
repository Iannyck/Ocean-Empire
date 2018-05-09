using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnchored : MonoBehaviour
{
    public bool repositionOnEnable = true;
    public bool repositionOnGameReady = true;

    [Header("1 = deep"), Range(0, 1)]
    public float depthAnchor;

    [Header("Offset")]
    public Vector2 xOffset;
    public float yOffset;

    void Start()
    {
        if (repositionOnGameReady)
            Game.OnGameReady += Reposition;
    }

    void OnEnable()
    {
        if (repositionOnEnable)
            Reposition();
    }

    public void Reposition()
    {
        if (Game.Instance == null || Game.Instance.MapLayout == null)
            return;

        var yPos = Game.Instance.MapLayout.GetMapHeightFromPosition01(depthAnchor);

        var tr = transform;
        tr.position = new Vector3(Random.Range(xOffset.x, xOffset.y), yOffset + yPos, tr.position.z);
    }

}
