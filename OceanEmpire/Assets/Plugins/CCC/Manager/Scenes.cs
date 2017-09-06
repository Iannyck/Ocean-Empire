using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CCC.Manager
{
    public class Scenes : BaseManager<Scenes>
    {
        class ScenePromise
        {
            public ScenePromise(string name, Action<Scene> callback)
            {
                this.name = name;
                this.callback = callback;
            }
            public string name;
            public Action<Scene> callback;
            public Scene scene;
        }

        static List<ScenePromise> loadingScenes = new List<ScenePromise>();

        public override void Init()
        {
            SceneManager.sceneLoaded += OnSceneLoading;
            CompleteInit();
        }

        #region QualityOfLife

        static public T FindRootObject<T>(Scene scene)
        {
            return scene.FindRootObject<T>();
        }

        #endregion

        #region Load/Unload Methods

        static public void Load(string name, LoadSceneMode mode = LoadSceneMode.Single, Action<Scene> callback = null, bool unique = true)
        {
            if (unique && Exists(name)) return;

            ScenePromise scenePromise = new ScenePromise(name, callback);
            loadingScenes.Add(scenePromise);
            SceneManager.LoadScene(name, mode);
        }

        static public void LoadAsync(string name, LoadSceneMode mode = LoadSceneMode.Single, Action<Scene> callback = null, bool unique = true)
        {
            if (unique && Exists(name)) return;

            ScenePromise scenePromise = new ScenePromise(name, callback);
            loadingScenes.Add(scenePromise);
            SceneManager.LoadSceneAsync(name, mode);
        }

        static public void UnloadAsync(string name)
        {
            SceneManager.UnloadSceneAsync(name);
        }

        static public bool Exists(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName) return true;
            }
            for (int i = 0; i < loadingScenes.Count; i++)
            {
                if (loadingScenes[i].name == sceneName) return true;
            }
            return false;
        }

        static public Scene GetActive(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName) return SceneManager.GetSceneAt(i);
            }
            throw new System.Exception("No active scene by that name: " + sceneName);
        }

        static public int SceneCount()
        {
            return SceneManager.sceneCount;
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
            if (promise.callback != null)
                promise.callback(promise.scene);

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
}
