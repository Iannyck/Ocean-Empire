using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public SceneInfo sceneInfo;
    [SerializeField] private bool loadOnStart = false;
    [SerializeField] private bool dontLoadDuplicate = true;
    [SerializeField] private bool loadAsync = false;

    [Serializable]
    public class SceneEvent : UnityEvent<Scene> { }
    public SceneEvent onLoadComplete;

    void Start()
    {
        if (loadOnStart)
            PersistentLoader.LoadIfNotLoaded(Load);
        else
            PersistentLoader.LoadIfNotLoaded();
    }

    public void Load()
    {
        Load(null);
    }
    public void Load(Action<Scene> callback)
    {
        if (dontLoadDuplicate && Scenes.IsActiveOrBeingLoaded(sceneInfo))
            return;

        Action<Scene> localCallback = (scene) =>
        {
            if (callback != null)
                callback(scene);
            if (onLoadComplete != null)
                onLoadComplete.Invoke(scene);
        };

        if (loadAsync)
            Scenes.LoadAsync(sceneInfo, localCallback);
        else
            Scenes.Load(sceneInfo, localCallback);
    }
}
