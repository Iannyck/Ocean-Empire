//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Events;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif
//namespace CCC.Manager
//{
//    [System.Serializable]
//    public class KeySave
//    {
//        public string keyName;
//        public KeyCode keycode = KeyCode.Exclaim;

//        public KeySave(string name, KeyCode keycode)
//        {
//            this.keyName = name;
//            this.keycode = keycode;
//        }
//    }

//    [System.Serializable]
//    public class Key
//    {
//        [SerializeField]
//        string name;
//        [SerializeField]
//        KeyCode keyCode = KeyCode.Exclaim;
//        public bool openInInspector = false;
//        public UnityEvent onModify = new UnityEvent();

//        public static bool CompareName(Key a, Key b)
//        {
//            return a.name == b.name;
//        }

//        public Key(string name, KeyCode keycode)
//        {
//            this.name = name;
//            this.keyCode = keycode;
//        }

//        public void Copy(Key key)
//        {
//            name = key.name;
//            keyCode = key.keyCode;
//        }

//        public KeyCode GetKeyCode() { return keyCode; }
//        public void SetKeyCode(KeyCode keycode)
//        {
//            this.keyCode = keycode;
//            onModify.Invoke();
//        }
//        public string GetName() { return name; }
//        public void SetName(string name)
//        {
//            if (Application.isPlaying)
//            {
//                Debug.LogError("Cannot modify the name of a Key in runtime.");
//                return;
//            }
//            this.name = name;
//        }

//        public bool GetDown()
//        {
//            return Input.GetKeyDown(keyCode);
//        }
//        public bool GetUp()
//        {
//            return Input.GetKeyUp(keyCode);
//        }
//        public bool Get()
//        {
//            return Input.GetKey(keyCode);
//        }
//    }

//    [CreateAssetMenu(menuName = "Input Bank")]
//    public class InputBank : ScriptableObject
//    {
//        [System.Serializable]
//        public class SaveClass
//        {
//            public List<KeySave> keySaves = new List<KeySave>();

//            public KeySave Get(Key key)
//            {
//                foreach (KeySave keySave in keySaves)
//                {
//                    if (keySave.keyName == key.GetName()) return keySave;
//                }
//                return null;
//            }

//            public void Set(Key key)
//            {
//                //Vérifier si la save existe déjà, si oui, la remplacer
//                KeySave keySave = Get(key);
//                if (keySave == null)
//                {
//                    keySave = new KeySave(key.GetName(), key.GetKeyCode());
//                    keySaves.Add(keySave);
//                }
//                else
//                {
//                    keySave.keycode = key.GetKeyCode();
//                }
//            }

//            public void CopyFrom(SaveClass save)
//            {
//                this.keySaves = save.keySaves;
//            }
//        }

//        public static string defaultKeysPath = "/defaultKeys.dat";
//        public static string playerKeysPath = "/playerKeys.dat";
//        //public Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
//        public List<Key> keys;
//        SaveClass save = new SaveClass();

//        public Key GetKeyByName(string name)
//        {
//            foreach (Key key in keys)
//            {
//                if (key.GetName() == name) return key;
//            }
//            return null;
//        }

//        public void SetKey(Key key, KeyCode to)
//        {
//            key.SetKeyCode(to);
//            save.Set(key);
//        }

//        public void MoveBy(Key key, int amount)
//        {
//            if (!keys.Contains(key)) return;

//            int a = keys.IndexOf(key);
//            int b = a + amount;
//            b = Mathf.Clamp(b, 0, keys.Count - 1);
//            if (a == b) return;

//            Key bKey = keys[b];

//            keys.Remove(key);
//            keys.Insert(b, key);
//            keys.Remove(bKey);
//            keys.Insert(a, bKey);
//        }

//        public void Save()
//        {
//            SaveTo(Application.persistentDataPath + playerKeysPath);
//        }

//        public void SaveAsDefaults()
//        {
//            save.keySaves = new List<KeySave>();
//            foreach (Key key in keys)
//            {
//                save.Set(key);
//            }
//            SaveTo(Application.persistentDataPath + defaultKeysPath);
//        }

//        public bool Load()
//        {
//            return LoadFrom(Application.persistentDataPath + playerKeysPath);
//        }

//        public void LoadDefaults()
//        {
//            LoadFrom(Application.persistentDataPath + defaultKeysPath);
//        }

//        public void ClearDefaults()
//        {
//            if (Application.isPlaying)
//            {
//                Debug.LogError("Should not SaveAsDefaults while application is running.");
//                return;
//            }

//            if (File.Exists(Application.persistentDataPath + defaultKeysPath))
//            {
//                File.Delete(Application.persistentDataPath + defaultKeysPath);
//            }
//            save.keySaves = new List<KeySave>();
//        }

//        private void SaveTo(string path)
//        {
//            BinaryFormatter bf = new BinaryFormatter();
//            FileStream file = File.Open(path, FileMode.OpenOrCreate);
//            bf.Serialize(file, save);
//            file.Close();
//        }

//        private bool LoadFrom(string path)
//        {
//            if (File.Exists(path))
//            {
//                //Load class
//                BinaryFormatter bf = new BinaryFormatter();
//                FileStream file = File.Open(path, FileMode.Open);
//                SaveClass saveCopy = (SaveClass)bf.Deserialize(file);
//                save.CopyFrom(saveCopy);

//                //Apply class to 'keys'
//                foreach (KeySave keySave in save.keySaves)
//                {
//                    Key key = GetKeyByName(keySave.keyName);
//                    if (key != null) key.SetKeyCode(keySave.keycode);
//                }
//                file.Close();

//                return true;
//            }
//            return false;
//        }
//    }

//#if UNITY_EDITOR
//    [CustomEditor(typeof(InputBank))]
//    public class InputBankEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            //base.OnInspectorGUI();
//            InputBank bank = target as InputBank;

//            //Create List
//            if (bank.keys == null)
//            {
//                bank.keys = new List<Key>();
//            }

//            //Keys
//            for (int i = 0; i < bank.keys.Count; i++)
//            {
//                if (i >= bank.keys.Count) continue;

//                Key key = bank.keys[i];

//                key.openInInspector = EditorGUILayout.Foldout(key.openInInspector, "key: " + key.GetName());

//                if (key.openInInspector)
//                {
//                    key.SetName(EditorGUILayout.TextField("name", key.GetName()));
//                    key.SetKeyCode((KeyCode)EditorGUILayout.EnumPopup("KeyCode", key.GetKeyCode()));
//                    GUILayout.BeginHorizontal();
//                    if (!Application.isPlaying && GUILayout.Button("Remove", GUILayout.Width(100)))
//                    {
//                        bank.keys.Remove(key); // Remove Key
//                    }
//                    if (GUILayout.Button("^", GUILayout.Width(30)))
//                    {
//                        bank.MoveBy(key, -1); // Move up
//                    }//
//                    if (GUILayout.Button("v", GUILayout.Width(30)))
//                    {
//                        bank.MoveBy(key, 1); // Move down
//                    }

//                    GUILayout.EndHorizontal();
//                    EditorGUILayout.Space();
//                }
//            }

//            //Add Key
//            EditorGUILayout.Space();
//            EditorGUILayout.Space();

//            if (!Application.isPlaying && GUILayout.Button("Add Key"))
//            {
//                bank.keys.Add(new Key("", KeyCode.Asterisk)); // Add
//            }
//            EditorGUILayout.Space();
//            EditorGUILayout.Space();

//            GUILayout.BeginHorizontal();
//            if (!Application.isPlaying && GUILayout.Button("Save as defaults"))
//            {
//                bank.SaveAsDefaults();
//                AssetDatabase.SaveAssets();
//                Debug.LogWarning("Default keys saved to: " + Application.persistentDataPath + InputBank.defaultKeysPath);
//            }

//            if (!Application.isPlaying && GUILayout.Button("Load from defaults"))
//            {
//                bank.LoadDefaults();
//                Debug.LogWarning("Default keys loaded from: " + Application.persistentDataPath + InputBank.defaultKeysPath);
//            }

//            if (!Application.isPlaying && GUILayout.Button("Clear defaults"))
//            {
//                bank.ClearDefaults();
//                Debug.LogWarning("Default keys loaded from: " + Application.persistentDataPath + InputBank.defaultKeysPath);
//            }
//            GUILayout.EndHorizontal();

//            EditorUtility.SetDirty(bank);
//        }
//    }
//#endif
//}
