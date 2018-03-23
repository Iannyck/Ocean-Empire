using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shack_MapManager : MonoBehaviour
{
    [SerializeField] Text mapNameText;
    [SerializeField] DataSaver dataSaver;
    [SerializeField] PrebuiltMapData _defaultMapData;
    [ReadOnly, SerializeField] int _mapIndex;

    private const string SAVEKEY_MAPDATA = "MapData";
    private const string SAVEKEY_MAPINDEX = "MapIndex";
    private const string PREBUILT_MAPDATA_NAME = "Maps/PrebuiltMapData_";
    private MapData _mapData;

    public event Action<MapData> OnChangeMap = (x) => { };

    void OnEnable()
    {
        dataSaver.OnReassignData += FetchMapData;

        if (!dataSaver.HasEverLoaded)
            dataSaver.LateLoad();
        else
            FetchMapData();
    }

    void OnDisable()
    {
        dataSaver.OnReassignData -= FetchMapData;
    }

    void FetchMapData()
    {
        _mapData = (MapData)dataSaver.GetObjectClone(SAVEKEY_MAPDATA);
        _mapIndex = dataSaver.GetInt(SAVEKEY_MAPINDEX);

        if (_mapData == null)
            ChangeMap(_mapIndex);
        else
            ApplyMapData(_mapData);
    }

    void ApplyMapData(MapData mapData)
    {
        if (mapNameText != null)
        {
            mapNameText.text = mapData.Name;
            mapNameText.enabled = true;
        }

        OnChangeMap(mapData);
    }

    public MapData GetMapData()
    {
        return _mapData ?? _defaultMapData.MapData;
    }

    public void SetMapData(MapData mapData)
    {
        _mapData = mapData;
        ApplyMapData(GetMapData());
    }

    public int MapIndex { get { return _mapIndex; } private set { _mapIndex = value; } }

    private void LoadMap()
    {
        var path = PREBUILT_MAPDATA_NAME + MapIndex.ToString();
        var prebuiltMapData = Resources.Load<PrebuiltMapData>(path);
        if (prebuiltMapData == null)
        {
            Debug.LogWarning("Aucune ressource nommée: " + path + ". Normalement, on génèrerait une map avec un algo," +
                " mais pour l'instant, nous allons prendre la map par défaut");
            SetMapData(_defaultMapData.MapData);
        }
        else
        {
            SetMapData(prebuiltMapData.MapData);
        }
    }

    public void ChangeMap(int newMapIndex)
    {
        MapIndex = newMapIndex;
        LoadMap();
    }

    public void ChangeMap_Next()
    {
        ChangeMap(MapIndex + 1);
    }
    public void ChangeMap_Previous()
    {
        ChangeMap(MapIndex - 1);
    }
}
