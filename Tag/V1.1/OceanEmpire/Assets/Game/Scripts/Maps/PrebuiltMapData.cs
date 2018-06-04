using System.Collections.Generic;
using UnityEngine;
using Questing;

[CreateAssetMenu(menuName = "Ocean Empire/Map/Prebuilt Map Data")]
public class PrebuiltMapData : ScriptableObject
{
    [SerializeField] SceneInfo _gameScene;
    [SerializeField] MapData _mapData = new MapData("Some Map");
    [SerializeField] List<Object> relatedQuests;


    public MapData MapData { get { return _mapData; } }

    public List<IQuestBuilder> GetRelatedQuestBuilders()
    {
        var list = new List<IQuestBuilder>(relatedQuests.Count);
        foreach (var quest in relatedQuests)
        {
            if (quest != null)
                list.Add((IQuestBuilder)quest);
        }
        return list;
    }

    void OnValidate()
    {
        while (relatedQuests.Count > 3)
        {
            relatedQuests.RemoveLast();
            Debug.LogError("Max 3 quests");
        }

        for (int i = 0; i < relatedQuests.Count; i++)
        {
            if (!(relatedQuests[i] is IQuestBuilder))
            {
                Debug.LogError("The object must inherit from IQuestBuilder");
                relatedQuests.RemoveAt(i);
                i--;
            }
        }

        if (_gameScene != null && _mapData.GameSceneName != _gameScene.SceneName)
        {
            _mapData.GameSceneName = _gameScene.SceneName;
        }
    }
}