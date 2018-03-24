using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class OceanSceneManager : SceneManager
    {
        public override object Create(string tag, ArrayList parameters)
        {
            return null;
        }

        public override void Delete(string tag, ArrayList parameters)
        {

        }

        public override object Read(string tag)
        {
            switch (tag)
            {
                default:
                    Debug.LogError("Failed to read \"" + tag + "\" within scene manager.");
                    return null;
                case "player":
                    return Game.Instance.Submarine;
                case "unit spawner":
                    return Game.Instance.UnitInstantiator;
                case "pending fish gpc":
                    return Game.Instance.PendingFishGPC;
                case "ui":
                    return Game.Instance.Recolte_UI;
            }
        }
        public override T Read<T>(string tag)
        {
            var result = Read(tag);
            if (result == null)
                return default(T);

            if (result is T)
            {
                return (T)result;
            }
            else
            {
                Debug.LogError("Failed to read appropriate type for \"" + tag + "\" within scene manager.   " +
                    "Expected: " + typeof(T).ToString() + "   Received: " + result.GetType().ToString());
                return default(T);
            }
        }

        public override object UpdateElement(string tag, ArrayList parameters)
        {
            return null;
        }
    }
}