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

    [SerializeField] Color defaultWaterColor = new Color(0, 192f / 255f, 1);
    [SerializeField] WaterLayer[] _waterLayers;

    public void SetWaterColor(Color color)
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

    public void ApplyMapData(MapData mapData)
    {
        SetWaterColor(mapData.ShallowColor);
    }

    void OnValidate()
    {
        if (!Application.isPlaying)
            SetWaterColor(defaultWaterColor);
    }
}
