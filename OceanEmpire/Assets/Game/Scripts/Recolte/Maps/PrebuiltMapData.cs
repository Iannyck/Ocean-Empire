using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Map/Prebuilt Map Data")]
public class PrebuiltMapData : ScriptableObject
{
    [SerializeField] SceneInfo _gameScene;
    [SerializeField]
    MapData _mapData = new MapData("Some Map");

    public MapData MapData { get { return _mapData; } }

    void OnValidate()
    {
        if (_gameScene != null && _mapData.GameSceneName != _gameScene.SceneName)
        {
            _mapData.GameSceneName = _gameScene.SceneName;
        }
    }
}