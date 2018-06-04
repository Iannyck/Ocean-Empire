using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Persistence;
using System;

public class PersistentLoader : MonoBehaviour
{
    [Reorderable]
    public List<UnityEngine.Object> persistentObjects;

    public static PersistentLoader instance;
    private const string ASSETNAME = "CCC/Persistent Loader";
    static List<System.Action> callbacks = new List<System.Action>();

    private InitQueue queue;
    private List<string> pendingObjects = new List<string>();

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
                string name = persistentObjects[i].name;
                pendingObjects.Add(name);

                persistentObjects[i] = manager.DuplicationBehavior();
                manager = persistentObjects[i] as IPersistent;

                Action registeration = queue.Register();

                //Init
                manager.Init(() =>
                {
                    pendingObjects.Remove(name);
                    registeration();
                });
            }
        }
        queue.MarkEnd();

        this.DelayedCall(() =>
        {
            if (!queue.IsOver)
            {
                Debug.Log("A manager is taking an abnormally long time to initialize.");
                for (int i = 0; i < pendingObjects.Count; i++)
                {
                    Debug.Log(pendingObjects[i] + " has not called 'onComplete' yet.");
                }
            }
        }, 2);
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
                persistentObjects[i] = gameObj.GetComponent<IPersistent>() as UnityEngine.Object;
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

        var obj = Resources.Load<GameObject>(ASSETNAME);
        if (obj == null)
        {
            Debug.LogError("Il doit y avoir un prefab nommé: " + ASSETNAME + " avec le script PersistentLoader dans le dossier /Resources");
        }
        else
            GameObject.Instantiate(obj);
    }
}
