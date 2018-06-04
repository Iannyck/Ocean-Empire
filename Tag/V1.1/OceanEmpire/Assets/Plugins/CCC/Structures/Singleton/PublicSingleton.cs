using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.DesignPattern
{
    public class PublicSingleton<T> : Singleton<T> where T : class
    {
        public static T Instance { get { return instance; } protected set { instance = value; } }
    }
}