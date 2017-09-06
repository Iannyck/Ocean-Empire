using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneExtensions
{
    static public T FindRootObject<T>(this Scene scene)
    {
        GameObject[] rootObjs = scene.GetRootGameObjects();
        for (int i = 0; i < rootObjs.Length; i++)
        {
            if (rootObjs[i].GetComponent<T>() != null)
                return rootObjs[i].GetComponent<T>();
        }
        return default(T);
    }
}
