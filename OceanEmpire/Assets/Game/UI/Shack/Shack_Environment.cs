using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack_Environment : MonoBehaviour
{
    [System.Serializable]
    private struct WaterLayer
    {
        public SpriteRenderer[] WaterRenderers;
        public float ToWhiteLerpValue;
    }

    [Header("TEST MODE")]
    public bool manualMode = false;

    [Header("Sky")]
    public Color manualSkyColorTop = new Color(7f / 255, 188f / 255f, 254 / 255f);
    public Color manualSkyColorCenter = new Color(16f / 255, 204f / 255f, 242f / 255f);
    public Color manualSkyColorBottom = new Color(1, 1, 1);

    [Header("Water")]
    public Color manualWaterColor = new Color(0, 192f / 255f, 1);

    [Header("Components"), SerializeField] WaterLayer[] _waterLayers;
    [SerializeField] TriColored skyColorizer;

    void Awake()
    {
        manualMode = false;
    }

    public void ApplyMapData(MapData mapData)
    {
        SetWaterColor(mapData.ShallowColor);
        SetSkyColor(mapData.SkyColorBottom, mapData.SkyColorCenter, mapData.SkyColorTop);
    }

    public void SetWaterColor(Color color)
    {
        if (!manualMode)
            ForceSetWaterColor(color);
    }

    private void ForceSetWaterColor(Color color)
    {
        if (_waterLayers == null)
            return;

        foreach (var layer in _waterLayers)
        {
            if (layer.WaterRenderers == null)
                continue;

            var c = Color.Lerp(color, Color.white, layer.ToWhiteLerpValue);
            foreach (var spr in layer.WaterRenderers)
            {
                spr.color = c;
            }
        }
    }

    public void SetSkyColor(Color bottom, Color center, Color top)
    {
        if (!manualMode)
            ForceSetSkyColor(bottom, center, top);
    }

    private void ForceSetSkyColor(Color bottom, Color center, Color top)
    {
        skyColorizer.SetColors(top, center, bottom);
        if (Application.isPlaying && PersistentCamera.GetCamera() != null)
            PersistentCamera.GetCamera().backgroundColor = top;
    }

    void Update()
    {
        if (manualMode)
            ApplyManualMode();
    }

    void OnValidate()
    {
        if (!Application.isPlaying)
            ApplyManualMode();
    }

    public void CopyManualModeFromMapData(MapData mapData)
    {
        manualWaterColor = mapData.ShallowColor;
        manualSkyColorTop = mapData.SkyColorTop;
        manualSkyColorCenter = mapData.SkyColorCenter;
        manualSkyColorBottom = mapData.SkyColorBottom;
    }

    public void ApplyManualMode()
    {
        ForceSetSkyColor(manualSkyColorBottom, manualSkyColorCenter, manualSkyColorTop);
        ForceSetWaterColor(manualWaterColor);
    }
}
