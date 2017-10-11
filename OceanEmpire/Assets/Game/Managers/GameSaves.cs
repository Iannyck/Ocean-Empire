using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using System;
using CCC.Utility;
using FullSerializer;
using FullInspector;

public class GameSaves : BaseManager<GameSaves>
{
    [fiInspectorOnly]
    public OpenSavesButton locationButton;
    public int saveVersion = 1;

    [Serializable]
    public class Data
    {
        public Dictionary<string, int> ints = new Dictionary<string, int>();
        public Dictionary<string, float> floats = new Dictionary<string, float>();
        public Dictionary<string, string> strings = new Dictionary<string, string>();
        public Dictionary<string, bool> bools = new Dictionary<string, bool>();
        public Dictionary<string, DateTime> dateTimes = new Dictionary<string, DateTime>();
    }

    public override void Init()
    {
        //Debug.LogWarning("GameSaves: load started");
        //LoadAllAsync(delegate()
        //{
        //    Debug.LogWarning("GameSaves: load complete");
        //});
        LoadAll();
        CompleteInit();
        //Debug.LogWarning("GameSaves: load complete");
    }

    private string GetPath()
    {
        return Application.persistentDataPath + "/v" + saveVersion + "_";
    }

    #region Get Value
    public int GetInt(Type type, string key, int defaultVal = 0)
    {
        Data data = TypeToData(type);
        if (data.ints.ContainsKey(key))
            return data.ints[key];
        else
            return defaultVal;
    }
    public float GetFloat(Type type, string key, float defaultVal)
    {
        Data data = TypeToData(type);
        if (data.floats.ContainsKey(key))
            return data.floats[key];
        else
            return defaultVal;
    }
    public string GetString(Type type, string key, string defaultVal = "")
    {
        Data data = TypeToData(type);
        if (data.strings.ContainsKey(key))
            return data.strings[key];
        else
            return defaultVal;
    }
    public bool GetBool(Type type, string key, bool defaultVal = false)
    {
        Data data = TypeToData(type);
        if (data.bools.ContainsKey(key))
            return data.bools[key];
        else
            return defaultVal;
    }

    public DateTime GetDateTime(Type type, string key, DateTime defaultVal = new DateTime())
    {
        Data data = TypeToData(type);
        if (data.dateTimes.ContainsKey(key))
            return data.dateTimes[key];
        else
            return defaultVal;
    }



    #endregion

    #region Set Value
    public void SetInt(Type type, string key, int value)
    {
        Data data = TypeToData(type);
        if (data.ints.ContainsKey(key))
            data.ints[key] = value;
        else
            data.ints.Add(key, value);
    }
    public void SetFloat(Type type, string key, float value)
    {
        Data data = TypeToData(type);
        if (data.floats.ContainsKey(key))
            data.floats[key] = value;
        else
            data.floats.Add(key, value);
    }
    public void SetString(Type type, string key, string value)
    {
        Data data = TypeToData(type);
        if (data.strings.ContainsKey(key))
            data.strings[key] = value;
        else
            data.strings.Add(key, value);
    }
    public void SetBool(Type type, string key, bool value)
    {
        Data data = TypeToData(type);
        if (data.bools.ContainsKey(key))
            data.bools[key] = value;
        else
            data.bools.Add(key, value);
    }

    public void SetDateTime(Type type, string key, DateTime value)
    {
        Data data = TypeToData(type);
        if (data.dateTimes.ContainsKey(key))
            data.dateTimes[key] = value;
        else
            data.dateTimes.Add(key, value);
    }


    #endregion

    #region Contains ?
    public bool ContainsInt(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.ints.ContainsKey(key);
    }

    public bool ContainsFloat(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.floats.ContainsKey(key);
    }

    public bool ContainsString(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.strings.ContainsKey(key);
    }

    public bool ContainsBool(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.bools.ContainsKey(key);
    }

    public bool ContainsDateTime(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.dateTimes.ContainsKey(key);
    }


    #endregion

    #region Save/Load
    public void LoadAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        LoadDataAsync(Type.Shop, queue.Register());
        LoadDataAsync(Type.Tutorial, queue.Register());
        LoadDataAsync(Type.FishPop, queue.Register());

        queue.MarkEnd();
    }

    public void LoadAll()
    {
        LoadData(Type.Shop);
        LoadData(Type.Tutorial);
        LoadData(Type.FishPop);
    }

    public void SaveAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        SaveDataAsync(Type.Shop, queue.Register());
        SaveDataAsync(Type.Tutorial, queue.Register());
        SaveDataAsync(Type.FishPop, queue.Register());

        queue.MarkEnd();
    }

    [InspectorButton()]
    public void SaveAll()
    {
        SaveData(Type.Shop);
        SaveData(Type.Tutorial);
        SaveData(Type.FishPop);
#if UNITY_EDITOR
        Debug.Log("All Data Saved");
#endif
    }

    public void LoadDataAsync(Type type, Action onLoadComplete)
    {
        string ext = TypeToFileName(type);
        string path = GetPath() + ext;

        //Exists ?
        if (Saves.Exists(path))
            Saves.ThreadLoad(GetPath() + ext,
                delegate (object graph)
                {
                    ApplyDataByType(type, (Data)graph);
                    if (onLoadComplete != null)
                        onLoadComplete();
                });
        else
        {
            //Nouveau fichier !
            NewOfType(type);
            SaveDataAsync(type, onLoadComplete);
        }

    }

    public void LoadData(Type type)
    {
        string ext = TypeToFileName(type);
        string path = GetPath() + ext;

        //Exists ?
        if (Saves.Exists(path))
        {
            object graph = Saves.InstantLoad(GetPath() + ext);
            print(GetPath() + ext);
            ApplyDataByType(type, (Data)graph);
        }
        else
        {
            //Nouveau fichier !
            NewOfType(type);
            SaveData(type);
        }
    }

    public void SaveDataAsync(Type type, Action onSaveComplete)
    {
        string ext = TypeToFileName(type);
        Data data = TypeToData(type);

        Saves.ThreadSave(GetPath() + ext, data, onSaveComplete);
    }

    public void SaveData(Type type)
    {
        string ext = TypeToFileName(type);
        Data data = TypeToData(type);
        Saves.InstantSave(GetPath() + ext, data);
    }

    public void ClearAllSaves()
    {
        ClearSave(Type.Shop);
        ClearSave(Type.Tutorial);
        ClearSave(Type.FishPop);
    }

    [InspectorButton()]
    public void ClearShop()
    {
        ClearSave(Type.Shop);
#if UNITY_EDITOR
        Debug.Log("Shop Cleared");
#endif
    }
    [InspectorButton()]
    public void ClearTutorial()
    {
        ClearSave(Type.Tutorial);
#if UNITY_EDITOR
        Debug.Log("Tutorial Cleared");
#endif
    }
    [InspectorButton()]
    public void ClearFishPop()
    {
        ClearSave(Type.FishPop);
#if UNITY_EDITOR
        Debug.Log("FishPop Cleared");
#endif
    }

    public void ClearSave(Type type)
    {
        Saves.Delete(GetPath() + TypeToFileName(type));
        NewOfType(type);
    }

    #endregion

    #region ADD NEW CATEGORIES HERE

    private const string SHOP_FILE = "shop.dat";
    private const string TUTORIAL_FILE = "tutorial.dat";
    private const string FISHPOP_FILE = "fishpop.dat";

    public enum Type { Shop, Tutorial, FishPop}

    [ShowInInspector]
    private Data shopData = new Data();
    [ShowInInspector]
    private Data tutorialData = new Data();
    [ShowInInspector]
    private Data fishPopData = new Data();

    private string TypeToFileName(Type type)
    {
        switch (type)
        {
            case Type.Shop:
                return SHOP_FILE;
            case Type.Tutorial:
                return TUTORIAL_FILE;
            case Type.FishPop:
                return FISHPOP_FILE;
            default:
                return "";
        }
    }

    private Data TypeToData(Type type)
    {
        switch (type)
        {
            case Type.Shop:
                return shopData;
            case Type.Tutorial:
                return tutorialData;
            case Type.FishPop:
                return fishPopData;

            default:
                return null;
        }
    }

    private void ApplyDataByType(Type type, Data newData)
    {
        switch (type)
        {
            case Type.Shop:
                shopData = newData;
                break;
            case Type.Tutorial:
                tutorialData = newData;
                break;
            case Type.FishPop:
                fishPopData = newData;
                break;

            default:
                break;
        }
    }

    private void NewOfType(Type type)
    {
        switch (type)
        {
            case Type.Shop:
                shopData = new Data();
                break;
            case Type.Tutorial:
                tutorialData = new Data();
                break;
            case Type.FishPop:
                fishPopData = new Data();
                break;

            default:
                break;
        }
    }
}
#endregion
