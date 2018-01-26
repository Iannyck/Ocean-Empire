using UnityEngine;
using System.Collections;
using FullInspector;

namespace CCC.DesignPattern
{
    public class FISingleton<T> : BaseBehavior where T : class
    {
        protected static T instance;

        protected override void Awake()
        {
            base.Awake();

            if (!(this is T))
            {
                Debug.LogError("Trying to make a Singleton<" + typeof(T).Name + "> but instance is a " + this.GetType().Name + ".");
                return;
            }

            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if ((object)instance == (object)this)
                instance = null;
        }
    }
}
