using System;
using UnityEngine;
using CCC.Persistence;

public class MapManager : MonoPersistent
{
    [SerializeField] bool logMapNames = false;
    [SerializeField] DataSaver dataSaver;
    [SerializeField] PrebuiltMapData _defaultMapData;
    [ReadOnly, SerializeField] int _mapIndex;
    public int MapIndex { get { return _mapIndex; } private set { _mapIndex = value; } }

    private const string SAVEKEY_MAPDATA = "MapData";
    private const string SAVEKEY_MAPINDEX = "MapIndex";
    private const string PREBUILT_MAPDATA_NAME = "Maps/PrebuiltMapData_";
    public MapData MapData { get; private set; }

    public event Action<int, MapData> OnMapSet = (x, y) => { };
    public static MapManager Instance { get; private set; }


    public override void Init(Action onComplete)
    {
        Instance = this;

        dataSaver.OnReassignData += Pull;

        if (!dataSaver.HasEverLoaded)
            dataSaver.Load(onComplete);
        else
        {
            Pull();
            onComplete();
        }
    }

    void OnDestroy()
    {
        if (dataSaver != null)
            dataSaver.OnReassignData -= Pull;
    }

    #region Data Saver Interactions
    void Pull()
    {
        // Pull from dataSaver
        MapIndex = dataSaver.GetInt(SAVEKEY_MAPINDEX);
        MapData = (MapData)dataSaver.GetObjectClone(SAVEKEY_MAPDATA);

        // If map data is null, load/create one
        if (MapData == null)
            MapData = GetMapDataFromIndex(MapIndex);

        // Log
        if (logMapNames)
            Debug.Log("Map: " + MapData.Name);

        // Event
        OnMapSet(MapIndex, MapData);
    }
    void PushAndSave()
    {
        dataSaver.SetObjectClone(SAVEKEY_MAPDATA, MapData);
        dataSaver.SetInt(SAVEKEY_MAPINDEX, MapIndex);
        dataSaver.LateSave();
    }
    #endregion

    public void SetMap(int mapIndex)
    {
        // Get Data
        MapIndex = mapIndex;
        MapData = GetMapDataFromIndex(MapIndex);

        // Save to disc
        PushAndSave();

        // Log
        if (logMapNames)
            Debug.Log("Map Manager's map: " + MapData.Name);

        // Event
        OnMapSet(MapIndex, MapData);
    }
    public void SetMap_Next()
    {
        SetMap(MapIndex + 1);
    }
    public void SetMap_Previous()
    {
        SetMap(MapIndex - 1);
    }

    private MapData GetMapDataFromIndex(int index)
    {
        // Load from path. If null, pick default

        var path = PREBUILT_MAPDATA_NAME + index.ToString();
        var prebuiltMapData = Resources.Load<PrebuiltMapData>(path);
        if (prebuiltMapData == null)
        {
            Debug.LogWarning("Aucune ressource nommée: " + path + ". Normalement, on génèrerait une map avec un algo," +
                " mais pour l'instant, nous allons prendre la map par défaut");
            return _defaultMapData.MapData;
        }
        else
        {
            return prebuiltMapData.MapData;
        }
    }
}
