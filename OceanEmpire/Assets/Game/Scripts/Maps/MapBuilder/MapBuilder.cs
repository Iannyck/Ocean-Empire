using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    // References
    [Header("References"), SerializeField] Transform _waterLayerContainer;
    [SerializeField] SpriteRenderer _waterLayerPrefab;
    [SerializeField] Transform _oceanFloorInstance;
    [SerializeField] MapLayout _mapLayout;

    // Settings
    [Header("Settings")]
    [SerializeField] float _layersSpacing = 1.5f;
    [SerializeField] float _maxLateralOffset = 1;
    [SerializeField] Color _shallowColor;
    [SerializeField] Color _deepColor;
    [SerializeField] float _oceanFloorOffset;
    [SerializeField] float _waterLayersStartOffset;


    private float WaterLayerTop { get { return -2f; } }

    public void ApplyMapData(MapData mapData)
    {
        SetColors(mapData.ShallowColor, mapData.DeepColor);
    }

    public void UpdateAll()
    {
        SetDepth(_mapLayout.Depth);
    }

    public void SetLateralOffset(float offset)
    {
        _maxLateralOffset = offset;
        ApplyPositions();
    }
    public void SetOceanFloorOffset(float offset)
    {
        _oceanFloorOffset = offset;
        ApplyOceanFloorPosition();
    }
    public void SetWaterLayersStartOffset(float offset)
    {
        _waterLayersStartOffset = offset;
        ApplyPositions();
    }

    public void SetLayerSpacing(float spacing)
    {
        _layersSpacing = spacing;
        ApplyPositions();
    }

    public void SetDepth(float depth)
    {
        var bottom = Mathf.Min(-depth, _waterLayersStartOffset);

        int layerCount = Mathf.CeilToInt((_waterLayersStartOffset - bottom) / _layersSpacing);

        if (_waterLayerContainer.childCount < layerCount)
        {
            // Add new layers
            for (int i = _waterLayerContainer.childCount; i < layerCount; i++)
            {
                var newLayer = _waterLayerPrefab.DuplicateGO(_waterLayerContainer);
                newLayer.sortingOrder = i;
                ApplyPositionTo(i);
            }
        }
        else if (_waterLayerContainer.childCount > layerCount)
        {
            // Remove extra layers
            for (int i = _waterLayerContainer.childCount - 1; i >= layerCount; i--)
            {
                if (Application.isPlaying)
                    Destroy(_waterLayerContainer.GetChild(i).gameObject);
                else
                    DestroyImmediate(_waterLayerContainer.GetChild(i).gameObject);
            }
        }

        ApplyColors();
        ApplyOceanFloorPosition();
    }

    public void SetColors(Color shallowColor, Color deepColor)
    {
        _shallowColor = shallowColor;
        _deepColor = deepColor;
        ApplyColors();
    }


    public void ClearWater()
    {
        for (int i = _waterLayerContainer.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(_waterLayerContainer.GetChild(i).gameObject);
            else
                DestroyImmediate(_waterLayerContainer.GetChild(i).gameObject);
        }
    }
    public void RebuildWater()
    {
        ClearWater();
        SetDepth(_mapLayout.Depth);
    }


    void ApplyPositions()
    {
        for (int i = 0; i < _waterLayerContainer.childCount; i++)
        {
            ApplyPositionTo(i);
        }
        ApplyOceanFloorPosition();
    }
    void ApplyPositionTo(int i)
    {
        _waterLayerContainer.GetChild(i).position = GetWaterLayerPosition(i);
    }
    void ApplyOceanFloorPosition()
    {
        if (_oceanFloorInstance != null)
        {
            _oceanFloorInstance.position = GetWaterLayerPosition(_waterLayerContainer.childCount) + Vector3.up * _oceanFloorOffset;
        }
    }
    void ApplyColors()
    {
        for (int i = 0; i < _waterLayerContainer.childCount; i++)
        {
            ApplyColorTo(i);
        }
    }
    void ApplyColorTo(int i)
    {
        var stdColor = Color.Lerp(_shallowColor, _deepColor, (float)i / (_waterLayerContainer.childCount - 1));
        _waterLayerContainer.GetChild(i).GetComponent<SpriteRenderer>().color = i % 2 == 0 ? stdColor : Color.Lerp(stdColor, Color.black, 0.05f);
    }
    Vector3 GetWaterLayerPosition(int index)
    {
        return new Vector3(UnityEngine.Random.Range(-_maxLateralOffset, _maxLateralOffset), _waterLayersStartOffset - _layersSpacing * index, 0);
    }
}