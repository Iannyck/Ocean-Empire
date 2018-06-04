using UnityEngine;

/// <typeparam name="T">yourself</typeparam>
public abstract class SelfSpawningSingleton<T> : MonoBehaviour where T : SelfSpawningSingleton<T>
{
    private static T _instance;

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    protected static T _Instance
    {
        get
        {
            CheckInstance();
            return _instance;
        }
    }

    protected static T GetRawInstance()
    {
        return _instance;
    }

    protected static void CheckInstance()
    {
        if (_instance == null)
        {
            var obj = new GameObject("t");
            obj.AddComponent<T>();
            obj.name = _instance.GameObjectName();
        }
    }

    protected abstract string GameObjectName();
}
