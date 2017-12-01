using UnityEngine;
using System.Collections;

public class PublicSingleton<T> : MonoBehaviour where T : class
{
    public static T instance;

    protected virtual void Awake()
    {
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
