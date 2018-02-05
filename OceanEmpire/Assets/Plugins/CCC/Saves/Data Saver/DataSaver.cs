using CCC.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;

[CreateAssetMenu(menuName = "CCC/Other/Data Saver")]
public class DataSaver : ScriptablePersistent
{
    [Suffix(".dat")] public string fileName = "someData";
    public const string FILE_EXTENSION = ".dat";

    [NonSerialized] private Data _data = new Data();
    private Data data
    {
        get { return _data; }
        set
        {
            _data = value;
            if (OnReassignData != null)
                OnReassignData();
        }
    }

    [Serializable]
    private class Data
    {
        public Dictionary<string, int> ints = new Dictionary<string, int>();
        public Dictionary<string, float> floats = new Dictionary<string, float>();
        public Dictionary<string, string> strings = new Dictionary<string, string>();
        public Dictionary<string, bool> bools = new Dictionary<string, bool>();
        public Dictionary<string, object> objects = new Dictionary<string, object>();
        public Dictionary<string, DateTime> dateTimes = new Dictionary<string, DateTime>();
    }

    public delegate void SaverEvent();

    /// <summary>
    /// Évenement appelé lorsqu'on réassigne les données (load/clear)
    /// </summary>
    public event SaverEvent OnReassignData;

    /// <summary>
    /// Read/Write operation queue. C'est une queue qui assure l'ordonnancement des opérations read/write
    /// </summary>
    private Queue<Action> rwoQueue = new Queue<Action>();

    public override void Init(Action onComplete)
    {
        Load();
        onComplete();
    }

    private string GetPath()
    {
        return Application.persistentDataPath + "/" + fileName + FILE_EXTENSION;
    }

    #region Get Value
    public int GetInt(string key, int defaultVal = 0)
    {
        if (data.ints.ContainsKey(key))
            return data.ints[key];
        else
            return defaultVal;
    }
    public float GetFloat(string key, float defaultVal = 0)
    {
        if (data.floats.ContainsKey(key))
            return data.floats[key];
        else
            return defaultVal;
    }
    public string GetString(string key, string defaultVal = "")
    {
        if (data.strings.ContainsKey(key))
            return data.strings[key];
        else
            return defaultVal;
    }
    public bool GetBool(string key, bool defaultVal = false)
    {
        if (data.bools.ContainsKey(key))
            return data.bools[key];
        else
            return defaultVal;
    }
    public object GetObjectClone(string key, object defaultVal = null)
    {
        if (data.objects.ContainsKey(key))
        {
            object result = data.objects[key];
            return result != null ? ObjectCopier.Clone(result) : null;
        }
        else
            return defaultVal;
    }

    public Dictionary<string, int>.KeyCollection GetIntKeys() { return data.ints.Keys; }
    public Dictionary<string, bool>.KeyCollection GetBoolKeys() { return data.bools.Keys; }
    public Dictionary<string, float>.KeyCollection GetFloatKeys() { return data.floats.Keys; }
    public Dictionary<string, string>.KeyCollection GetStringKeys() { return data.strings.Keys; }
    public Dictionary<string, object>.KeyCollection GetObjectKeys() { return data.objects.Keys; }
    #endregion

    #region Set Value
    public void SetInt(string key, int value)
    {
        if (data.ints.ContainsKey(key))
            data.ints[key] = value;
        else
            data.ints.Add(key, value);
    }
    public void SetFloat(string key, float value)
    {
        if (data.floats.ContainsKey(key))
            data.floats[key] = value;
        else
            data.floats.Add(key, value);
    }
    public void SetString(string key, string value)
    {
        if (data.strings.ContainsKey(key))
            data.strings[key] = value;
        else
            data.strings.Add(key, value);
    }
    public void SetBool(string key, bool value)
    {
        if (data.bools.ContainsKey(key))
            data.bools[key] = value;
        else
            data.bools.Add(key, value);
    }
    public void SetObjectClone(string key, object value)
    {
        object clone = value != null ? ObjectCopier.Clone(value) : null;

        if (data.objects.ContainsKey(key))
            data.objects[key] = clone;
        else
            data.objects.Add(key, clone);
    }
    #endregion

    #region Delete
    public bool DeleteInt(string key)
    {
        return data.ints.Remove(key);
    }
    public bool DeleteFloat(string key)
    {
        return data.floats.Remove(key);
    }
    public bool DeleteString(string key)
    {
        return data.strings.Remove(key);
    }
    public bool DeleteBool(string key)
    {
        return data.bools.Remove(key);
    }
    public bool DeleteObjectClone(string key)
    {
        return data.objects.Remove(key);
    }
    #endregion

    #region Contains ?
    public bool ContainsInt(string key)
    {
        return data.ints.ContainsKey(key);
    }

    public bool ContainsFloat(string key)
    {
        return data.floats.ContainsKey(key);
    }

    public bool ContainsString(string key)
    {
        return data.strings.ContainsKey(key);
    }

    public bool ContainsBool(string key)
    {
        return data.bools.ContainsKey(key);
    }

    public bool ContainsObject(string key)
    {
        return data.objects.ContainsKey(key);
    }
    #endregion

    #region Save/Load

    public void LoadAsync() { LoadAsync(null); }
    public void LoadAsync(Action onLoadComplete)
    {
        AddRWOperation(() =>
        {
            string path = GetPath();

            //Exists ?
            if (Saves.Exists(path))
                Saves.ThreadLoad(path,
                    delegate (object graph)
                    {
                        data = (Data)graph;

                        if (onLoadComplete != null)
                            onLoadComplete();

                        CompleteRWOperation();
                    });
            else
            {
                //Nouveau fichier !
                data = new Data();
                SaveAsync(onLoadComplete);

                CompleteRWOperation();
            }
        });
    }

    public void Load() { Load(null); }
    public void Load(Action onLoadComplete)
    {
        AddRWOperation(() =>
        {
            string path = GetPath();

            //Exists ?
            if (Saves.Exists(path))
            {
                //Load and apply
                object graph = Saves.InstantLoad(path);
                data = (Data)graph;

                if (onLoadComplete != null)
                    onLoadComplete();
            }
            else
            {
                //Nouveau fichier !
                data = new Data();

                Save(onLoadComplete);
            }

            CompleteRWOperation();
        });
    }

    public void SaveAsync() { SaveAsync(null); }
    public void SaveAsync(Action onSaveComplete)
    {
        AddRWOperation(() =>
        {
            Saves.ThreadSave(GetPath(), data, () =>
            {
                if (onSaveComplete != null)
                    onSaveComplete();

                CompleteRWOperation();
            });
        });
    }

    public void Save() { Save(null); }
    public void Save(Action onSaveComplete)
    {
        AddRWOperation(() =>
        {
            Saves.InstantSave(GetPath(), data);

            if (onSaveComplete != null)
                onSaveComplete();

            CompleteRWOperation();
        });
    }

    public void ClearSave() { ClearSave(null); }
    public void ClearSave(Action onComplete)
    {
        AddRWOperation(() =>
        {
            Saves.Delete(GetPath());
            data = new Data();

            if (onComplete != null)
                onComplete();

            CompleteRWOperation();
        });
    }

    #region RW Operations
    private void AddRWOperation(Action action)
    {
        //On s'enfile
        rwoQueue.Enqueue(action);

        //Sommes nous les premier a etre dans la queue ?
        if (rwoQueue.Count == 1)
            action();
    }

    private void CompleteRWOperation()
    {
        //On enleve la derniere action
        rwoQueue.Dequeue();

        //On execute la prochaine s'il y en a une
        if (rwoQueue.Count > 0)
            rwoQueue.Peek()();
    }
    #endregion

    #endregion
}
