using UnityEngine;
using System.Collections;
using FullInspector;

namespace CCC.DesignPattern
{
    public class PublicFISingleton<T> : FISingleton<T> where T : class
    {
        public static T Instance { get { return instance; } protected set { instance = value; } }
    }
}