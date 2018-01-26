using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;

public class PersistentLoader : MonoBehaviour
{
    [Reorderable]
    public List<Object> persistentObjects;

    public static PersistentLoader instance;
    private const string AssetName = "Persistent Loader";
    static List<System.Action> callbacks = new List<System.Action>();

    private InitQueue queue;

    static public void LoadIfNotLoaded(System.Action callback = null)
    {
        LoadInstanceIfNeeded();

        if (callback != null)
        {
            if (instance.queue.IsOver)
                callback();
            else
                callbacks.Add(callback);
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        queue = new InitQueue(() =>
        {
            //On complete
            for (int i = 0; i < callbacks.Count; i++)
            {
                callbacks[i]();
            }
            callbacks = null;
        });

        for (int i = 0; i < persistentObjects.Count; i++)
        {
            if (persistentObjects[i] == null)
                continue;

            IPersistent manager = persistentObjects[i] as IPersistent;
            if (manager != null)
            {
                persistentObjects[i] = manager.DuplicationBehavior();
                manager = persistentObjects[i] as IPersistent;

                //Init
                manager.Init(queue.Register());
            }
        }
        queue.MarkEnd();
    }

    void OnValidate()
    {
        for (int i = 0; i < persistentObjects.Count; i++)
        {
            if (persistentObjects[i] == null)
                continue;

            GameObject gameObj = persistentObjects[i] as GameObject;
            if (gameObj != null)
            {
                persistentObjects[i] = gameObj.GetComponent<IPersistent>() as Object;
            }

            if (!(persistentObjects[i] is IPersistent))
            {
                persistentObjects[i] = null;
                Debug.LogWarning("L'objet doit hériter de IPersistent");
            }
        }
    }

    static void LoadInstanceIfNeeded()
    {
        if (instance != null) return;

        var obj = Resources.Load<GameObject>(AssetName);
        if (obj == null)
        {
            Debug.LogError("Il doit y avoir un prefab nommé: " + AssetName + " avec le script PersistentLoader dans le dossier /Resources");
        }
        else
            GameObject.Instantiate(obj);
    }
}
