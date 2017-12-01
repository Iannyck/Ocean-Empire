//using UnityEngine;
//using System.Collections;
//using UnityEngine.Events;
//using System.Collections.Generic;

//namespace CCC.Manager
//{
//    public class InputManager : BaseManager<InputManager>
//    {
//        [SerializeField]
//        private InputBank bank;

//        public override void Init()
//        {
//            bank.SaveAsDefaults();
//            Load();
//            CompleteInit();
//        }

//        /// <summary>
//        /// Set the specified key to the specified keycode. This does not save the change permanently. Call Save() if necessary.
//        /// </summary>
//        public void SetKey(Key key, KeyCode to)
//        {
//            bank.SetKey(key, to);
//        }

//        /// <summary>
//        /// Return TRUE if there is a conlfict.
//        /// </summary>
//        public bool CheckConflict(Key key, KeyCode newKeycode)
//        {
//            foreach(Key akey in bank.keys)
//            {
//                if (akey != key && akey.GetKeyCode() == newKeycode) return true;
//            }
//            return false;
//        }

//        public Key GetKeyByName(string name)
//        {
//            return bank.GetKeyByName(name);
//        }

//        public List<Key> GetAllKeys() { return bank.keys; }

//        #region Load/Save

//        public void LoadDefaults()
//        {
//            bank.LoadDefaults();
//        }

//        public void Load()
//        {
//            bank.Load();
//        }

//        public void Save()
//        {
//            bank.Save();
//        }

//        #endregion
//    }
//}
