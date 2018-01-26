using System;
using System.Collections;
using System.Collections.Generic;
using CCC.Persistence;
using UnityEngine.SceneManagement;

public class Scenes : MonoPersistent
{
    class ScenePromise
    {
        public ScenePromise(string name, Action<Scene> callback)
        {
            this.name = name;
            Callback += callback;
        }
        public string name;
        public event Action<Scene> Callback;
        public Scene scene;
        public void InvokeCallback()
        {
            if (Callback != null)
                Callback(scene);
        }
    }

    static List<ScenePromise> loadingScenes = new List<ScenePromise>();
    private static Scenes instance;

    public override void Init(Action onComplete)
    {
        SceneManager.sceneLoaded += OnSceneLoading;
        onComplete();
    }

    void Awake()
    {
        instance = this;
    }

    #region QualityOfLife

    static public T FindRootObject<T>(Scene scene)
    {
        return scene.FindRootObject<T>();
    }

    #endregion

    #region Load/Unload Methods

    static public void Load(string name, LoadSceneMode mode = LoadSceneMode.Single, Action<Scene> callback = null, bool allowMultiple = true)
    {
        if (allowMultiple && _HandleUniqueLoad(name, callback))
            return;

        ScenePromise scenePromise = new ScenePromise(name, callback);
        loadingScenes.Add(scenePromise);
        SceneManager.LoadScene(name, mode);
    }
    static public void Load(SceneInfo sceneInfo, Action<Scene> callback = null)
    {
        Load(sceneInfo.SceneName, sceneInfo.LoadMode, callback, sceneInfo.AllowMultiple);
    }

    static public void LoadAsync(string name, LoadSceneMode mode = LoadSceneMode.Single, Action<Scene> callback = null, bool allowMultiple = true)
    {
        if (allowMultiple && _HandleUniqueLoad(name, callback))
            return;

        ScenePromise scenePromise = new ScenePromise(name, callback);
        loadingScenes.Add(scenePromise);
        SceneManager.LoadSceneAsync(name, mode);
    }
    static public void LoadAsync(SceneInfo sceneInfo, Action<Scene> callback = null)
    {
        LoadAsync(sceneInfo.SceneName, sceneInfo.LoadMode, callback, sceneInfo.AllowMultiple);
    }

    static private bool _HandleUniqueLoad(string sceneName, Action<Scene> callback)
    {
        if (IsBeingLoaded(sceneName))
        {
            GetLoadingScene(sceneName).Callback += callback;
            return true;
        }
        else if (IsActive(sceneName))
        {
            if (callback != null)
                callback(GetActive(sceneName));
            return true;
        }
        return false;
    }

    static public void UnloadAsync(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }
    static public void UnloadAsync(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
    static public void UnloadAsync(SceneInfo sceneInfo)
    {
        UnloadAsync(sceneInfo.SceneName);
    }

    static public bool IsActiveOrBeingLoaded(string sceneName)
    {
        if (IsActive(sceneName) || IsBeingLoaded(sceneName))
            return true;
        return false;
    }
    static public bool IsActiveOrBeingLoaded(SceneInfo sceneInfo)
    {
        return IsActiveOrBeingLoaded(sceneInfo.SceneName);
    }

    static public bool IsBeingLoaded(string sceneName)
    {
        for (int i = 0; i < loadingScenes.Count; i++)
        {
            if (loadingScenes[i].name == sceneName) return true;
        }
        return false;
    }
    static public bool IsBeingLoaded(SceneInfo sceneInfo)
    {
        return IsBeingLoaded(sceneInfo.SceneName);
    }

    static public bool IsActive(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName) return true;
        }
        return false;
    }
    static public bool IsActive(SceneInfo sceneInfo)
    {
        return IsActive(sceneInfo.SceneName);
    }

    static public Scene GetActive(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName) return SceneManager.GetSceneAt(i);
        }
        throw new System.Exception("No active scene by that name: " + sceneName);
    }
    static public Scene GetActive(SceneInfo sceneInfo)
    {
        return GetActive(sceneInfo.SceneName);
    }

    static public int ActiveSceneCount
    {
        get { return SceneManager.sceneCount; }
    }

    static public int LoadingSceneCount
    {
        get { return loadingScenes.Count; }
    }

    #endregion

    #region InLoading Events

    static void OnSceneLoading(Scene scene, LoadSceneMode mode)
    {
        ScenePromise promise = GetLoadingScene(scene.name);
        if (promise == null) return;

        promise.scene = scene;

        if (!scene.isLoaded) instance.StartCoroutine(WaitForSceneLoad(scene, promise));
        else Execute(promise);
    }

    static IEnumerator WaitForSceneLoad(Scene scene, ScenePromise promise)
    {
        while (!scene.isLoaded)
            yield return null;
        Execute(promise);
    }

    static void Execute(ScenePromise promise)
    {
        promise.InvokeCallback();

        loadingScenes.Remove(promise);
    }

    #endregion

    #region Internal Utility

    static ScenePromise GetLoadingScene(string name)
    {
        foreach (ScenePromise scene in loadingScenes)
        {
            if (scene.name == name) return scene;
        }
        return null;
    }

    #endregion

}