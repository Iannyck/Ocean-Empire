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
        public Dictionary<string, object> objects = new Dictionary<string, object>();
    }

    /// <summary>
    /// Read/Write operation queue. C'est une queue qui assure l'ordonnancement des operation read/write
    /// </summary>
    private Dictionary<Type, Queue<Action>> rwoQueue = new Dictionary<Type, Queue<Action>>();

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

    public object GetObjectClone(Type type, string key, object defaultVal = null)
    {
        Data data = TypeToData(type);
        if (data.objects.ContainsKey(key))
        {
            object result = data.objects[key];
            return result != null ? ObjectCopier.Clone(result) : null;
        }
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

    public void SetObjectClone(Type type, string key, object value)
    {
        object clone = value != null ? ObjectCopier.Clone(value) : null;

        Data data = TypeToData(type);
        if (data.objects.ContainsKey(key))
            data.objects[key] = clone;
        else
            data.objects.Add(key, clone);
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

    public bool ContainsObject(Type type, string key)
    {
        Data data = TypeToData(type);
        return data.objects.ContainsKey(key);
    }


    #endregion

    #region Save/Load
    public void LoadAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        LoadDataAsync(Type.Currency, queue.Register());
        LoadDataAsync(Type.Tutorial, queue.Register());
        LoadDataAsync(Type.FishPop, queue.Register());
        LoadDataAsync(Type.Items, queue.Register());
        LoadDataAsync(Type.Calendar, queue.Register());
        LoadDataAsync(Type.History, queue.Register());

        queue.MarkEnd();
    }

    public void LoadAll()
    {
        LoadData(Type.Currency);
        LoadData(Type.Tutorial);
        LoadData(Type.FishPop);
        LoadData(Type.Items);
        LoadData(Type.Calendar);
        LoadData(Type.History);
    }

    public void SaveAllAsync(Action onComplete)
    {
        InitQueue queue = new InitQueue(onComplete);
        SaveDataAsync(Type.Currency, queue.Register());
        SaveDataAsync(Type.Tutorial, queue.Register());
        SaveDataAsync(Type.FishPop, queue.Register());
        SaveDataAsync(Type.Items, queue.Register());
        SaveDataAsync(Type.Calendar, queue.Register());
        SaveDataAsync(Type.History, queue.Register());

        queue.MarkEnd();
    }

    [InspectorButton()]
    public void SaveAll()
    {
        SaveData(Type.Currency);
        SaveData(Type.Tutorial);
        SaveData(Type.FishPop);
        SaveData(Type.Items);
        SaveData(Type.Calendar);
        SaveData(Type.History);

#if UNITY_EDITOR
        Debug.Log("All Data Saved");
#endif
    }

    public void LoadDataAsync(Type type, Action onLoadComplete)
    {
        AddRWOperation(type, () =>
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

                        CompleteRWOperation(type);
                    });
            else
            {
                //Nouveau fichier !
                NewOfType(type);
                SaveDataAsync(type, onLoadComplete);

                CompleteRWOperation(type);
            }
        });
    }

    public void LoadData(Type type, Action onLoadComplete = null)
    {
        AddRWOperation(type, () =>
        {
            string ext = TypeToFileName(type);
            string path = GetPath() + ext;

            //Exists ?
            if (Saves.Exists(path))
            {
                object graph = Saves.InstantLoad(GetPath() + ext);
                ApplyDataByType(type, (Data)graph);
            }
            else
            {
                //Nouveau fichier !
                NewOfType(type);
                SaveData(type);
            }

            CompleteRWOperation(type);
        });
    }

    public void SaveDataAsync(Type type, Action onSaveComplete)
    {
        AddRWOperation(type, () =>
        {
            string ext = TypeToFileName(type);
            Data data = TypeToData(type);

            Saves.ThreadSave(GetPath() + ext, data, () =>
            {
                if (onSaveComplete != null)
                    onSaveComplete();

                CompleteRWOperation(type);
            });
        });
    }

    public void SaveData(Type type, Action onSaveComplete = null)
    {
        AddRWOperation(type, () =>
        {
            string ext = TypeToFileName(type);
            Data data = TypeToData(type);
            Saves.InstantSave(GetPath() + ext, data);

            if (onSaveComplete != null)
                onSaveComplete();

            CompleteRWOperation(type);
        });
    }

    private void AddRWOperation(Type type, Action action)
    {
        //S'il y a deja une queue, on s'enfile et on attend
        if (rwoQueue.ContainsKey(type))
        {
            //On s'enfile
            rwoQueue[type].Enqueue(action);
        }
        else
        {
            //On cree la queue et execute l'operation
            rwoQueue.Add(type, new Queue<Action>());
            action();
        }
    }

    private void CompleteRWOperation(Type type)
    {
        if (rwoQueue.ContainsKey(type))
        {
            Queue<Action> q = rwoQueue[type];
            if (q.Count == 0)
            {
                //On est au bout de la file
                rwoQueue.Remove(type);
            }
            else
            {
                //On execute la prochain action
                Action nextOperation = q.Dequeue();
                nextOperation();
            }
        }
        else
        {
            Debug.LogError("Ne devrais pas arriver");
        }
    }

    public void ClearAllSaves()
    {
        ClearCurrency();
        ClearTutorial();
        ClearFishPop();
        ClearItems();
        ClearCalendar();
        ClearHistory();
    }

    [InspectorButton()]
    public void ClearCurrency()
    {
        ClearSave(Type.Currency);
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
    [InspectorButton()]
    public void ClearItems()
    {
        ClearSave(Type.Items);
#if UNITY_EDITOR
        Debug.Log("Items Cleared");
#endif
    }
    [InspectorButton()]
    public void ClearCalendar()
    {
        ClearSave(Type.Calendar);

        if (Calendar.instance)
            Calendar.instance.Reload();

#if UNITY_EDITOR
        Debug.Log("Calendar Cleared");
#endif
    }
    [InspectorButton()]
    public void ClearHistory()
    {
        ClearSave(Type.History);

        if (History.instance)
            History.instance.Reload();

#if UNITY_EDITOR
        Debug.Log("History Cleared");
#endif
    }

    public void ClearSave(Type type)
    {
        Saves.Delete(GetPath() + TypeToFileName(type));
        NewOfType(type);
    }


    #endregion

    #region ADD NEW CATEGORIES HERE

    private const string CURRENCY_FILE = "currency.dat";
    private const string TUTORIAL_FILE = "tutorial.dat";
    private const string FISHPOP_FILE = "fishpop.dat";
    private const string ITEMS_FILE = "items.dat";
    private const string CALENDAR_FILE = "calendar.dat";
    private const string HISTORY_FILE = "history.dat";

    public enum Type { Currency = 0, Tutorial = 1, FishPop = 2, Items = 3, Calendar = 4, History = 5 }

    [ShowInInspector]
    private Data currencyData = new Data();
    [ShowInInspector]
    private Data tutorialData = new Data();
    [ShowInInspector]
    private Data fishPopData = new Data();
    [ShowInInspector]
    private Data itemsData = new Data();
    [ShowInInspector]
    private Data calendarData = new Data();
    [ShowInInspector]
    private Data historyData = new Data();

    private string TypeToFileName(Type type)
    {
        switch (type)
        {
            case Type.Currency:
                return CURRENCY_FILE;
            case Type.Tutorial:
                return TUTORIAL_FILE;
            case Type.FishPop:
                return FISHPOP_FILE;
            case Type.Items:
                return ITEMS_FILE;
            case Type.Calendar:
                return CALENDAR_FILE;
            case Type.History:
                return HISTORY_FILE;
            default:
                return "";
        }
    }

    private Data TypeToData(Type type)
    {
        switch (type)
        {
            case Type.Currency:
                return currencyData;
            case Type.Tutorial:
                return tutorialData;
            case Type.FishPop:
                return fishPopData;
            case Type.Items:
                return itemsData;
            case Type.Calendar:
                return calendarData;
            case Type.History:
                return historyData;
            default:
                return null;
        }
    }

    private void ApplyDataByType(Type type, Data newData)
    {
        switch (type)
        {
            case Type.Currency:
                currencyData = newData;
                break;
            case Type.Tutorial:
                tutorialData = newData;
                break;
            case Type.FishPop:
                fishPopData = newData;
                break;
            case Type.Items:
                itemsData = newData;
                break;
            case Type.Calendar:
                calendarData = newData;
                break;
            case Type.History:
                historyData = newData;
                break;
            default:
                break;
        }
    }

    private void NewOfType(Type type)
    {
        switch (type)
        {
            case Type.Currency:
                currencyData = new Data();
                break;
            case Type.Tutorial:
                tutorialData = new Data();
                break;
            case Type.FishPop:
                fishPopData = new Data();
                break;
            case Type.Items:
                itemsData = new Data();
                break;
            case Type.Calendar:
                calendarData = new Data();
                break;
            case Type.History:
                historyData = new Data();
                break;
            default:
                break;
        }
    }
}
#endregion
