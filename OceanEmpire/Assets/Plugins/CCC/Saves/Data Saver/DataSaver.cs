using CCC.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using CCC.Serialization;

[CreateAssetMenu(menuName = "CCC/Other/Data Saver")]
public class DataSaver : FileScriptableInterface, IPersistent// ScriptablePersistent
{
    [NonSerialized] private Data data = new Data();

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

    #region IPersistent
    public void Init(Action onComplete)
    {
        Load();
        onComplete();
    }

    public UnityEngine.Object DuplicationBehavior()
    {
        return this;
    }
    #endregion

    #region File Scriptable Interface
    protected override object GetLocalData()
    {
        return data;
    }
    protected override void OverwriteLocalData(object graph)
    {
        if (graph == null)
        {
            SetDefaultLocalData();
        }
        else
        {
            data = (Data)graph;
        }
    }
    protected override void SetDefaultLocalData()
    {
        data = new Data();
    }
    #endregion

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
}
