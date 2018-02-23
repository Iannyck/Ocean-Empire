using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GPComponents
{
    public abstract class SceneManager : MonoBehaviour
    {
        public abstract object Create(string tag, ArrayList parameters);

        public abstract object Read(string tag);
        public abstract T Read<T>(string tag);

        public abstract object UpdateElement(string tag, ArrayList parameters);

        public abstract void Delete(string tag, ArrayList parameters);

    }
}

