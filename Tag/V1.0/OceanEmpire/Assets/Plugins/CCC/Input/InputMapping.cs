using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CCC.Utility;
using FullInspector;

namespace CCC.Input
{
    [CreateAssetMenu(menuName = "CCC/InputMapping")]
    public class InputMapping : BaseScriptableObject
    {
        public string displayName;
        [InspectorDivider(), fiInspectorOnly]
        public OpenSavesButton locationButton;
        public Dictionary<string, InputKey> keys;

        const string midDefault = "/IM_default_";
        const string mid = "/IM_save_";
        const string ext = ".dat";

        private string NormalPath
        {
            get { return Application.persistentDataPath + mid + name + ext; }
        }
        private string DefaultPath
        {
            get { return Application.persistentDataPath + midDefault + name + ext; }
        }

        public void SaveAsDefaults()
        {
            Saves.InstantSave(DefaultPath, keys);
            if (Saves.Exists(NormalPath))
                Save();
        }

        public void Save()
        {
            Saves.InstantSave(NormalPath, keys);
        }
        public bool LoadDefaults()
        {
            object graph = Saves.InstantLoad(DefaultPath);

            if (graph == null)
                return false;

            keys = (Dictionary<string, InputKey>)graph;
            return true;
        }
        public bool Load()
        {
            object graph = Saves.InstantLoad(NormalPath);

            if (graph == null)
                return false;

            keys = (Dictionary<string, InputKey>)graph;
            return true;
        }

        public bool Delete()
        {
            return Saves.Delete(NormalPath);
        }
        public bool DeleteDefaults()
        {
            return Saves.Delete(DefaultPath);
        }

        [InspectorButton(), InspectorName("Save")]
        public void EditorSave()
        {
            Save();
            Debug.LogWarning("Save successful");
        }
        [InspectorButton(), InspectorName("Save as Defaults")]
        public void EditorSaveDefaults()
        {
            SaveAsDefaults();
            Debug.LogWarning("Default save successful");
        }

        [InspectorButton(), InspectorName("Load Save")]
        public void EditorLoad()
        {
            if (!Load())
                Debug.LogWarning("Failed to load a save. No file found.");
        }

        [InspectorButton(), InspectorName("Load Defaults")]
        public void EditorLoadDefaults()
        {
            if (!LoadDefaults())
                Debug.LogWarning("Failed to load defaults. No file found.");
        }

        [InspectorButton(), InspectorName("Delete Save")]
        public void EditorDelete()
        {
            if (!Delete())
                Debug.LogWarning("Failed to delete a save. No file found.");
        }
        [InspectorButton(), InspectorName("Delete Defaults")]
        public void EditorDeleteDefaults()
        {
            if (!DeleteDefaults())
                Debug.LogWarning("Failed to delete defaults. No file found.");
        }
    }

    [System.Serializable]
    public class InputKey
    {
        public string displayCategory = "";
        public KeyCombination primary = null;
        public KeyCombination secondary = null;

        private bool GetPrimary { get { return primary != null && primary.Get(); } }
        private bool GetPrimaryDown { get { return primary != null && primary.GetDown(); } }
        private bool GetPrimaryUp { get { return primary != null && primary.GetUp(); } }
        private bool GetSecondary { get { return secondary != null && secondary.Get(); } }
        private bool GetSecondaryDown { get { return secondary != null && secondary.GetDown(); } }
        private bool GetSecondaryUp { get { return secondary != null && secondary.GetUp(); } }

        public bool Get()
        {
            return GetPrimary || GetSecondary;
        }
        public bool GetDown()
        {
            return GetPrimaryDown || GetSecondaryDown;
        }
        public bool GetUp()
        {
            return GetPrimaryUp || GetSecondaryUp;
        }
    }

    [System.Serializable]
    public class KeyCombination
    {
        public KeyCode first;
        public KeyCode second;
        public KeyCombination(KeyCode first, KeyCode second) { this.first = first; this.second = second; }
        public bool Get()
        {
            if (second == KeyCode.None)
                return UnityEngine.Input.GetKey(first);
            else
                return UnityEngine.Input.GetKey(first) && UnityEngine.Input.GetKey(second);
        }
        public bool GetDown()
        {
            if (second == KeyCode.None)
                return UnityEngine.Input.GetKeyDown(first);
            else
                return (UnityEngine.Input.GetKey(first) || UnityEngine.Input.GetKeyDown(first)) && UnityEngine.Input.GetKeyDown(second);
        }
        public void Copy(KeyCombination newCombination)
        {
            first = newCombination.first;
            second = newCombination.second;
        }
        public bool GetUp()
        {
            if (second == KeyCode.None)
                return UnityEngine.Input.GetKeyUp(first);
            else
            {
                bool fu = UnityEngine.Input.GetKeyUp(first);
                bool f = UnityEngine.Input.GetKey(first);
                bool su = UnityEngine.Input.GetKeyUp(second);
                bool s = UnityEngine.Input.GetKey(second);

                return (fu && (s || su)) || (su && (f || fu));
            }
        }
    }
}