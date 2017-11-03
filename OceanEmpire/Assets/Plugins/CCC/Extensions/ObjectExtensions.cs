using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectExtensions
{
    /// <summary>
    ///Instantiate une copie du gameobject
    /// </summary>
    public static T DuplicateGO<T>(this T original) where T : Component
    {
        return GameObject.Instantiate(original.gameObject).GetComponent<T>();
    }

    /// <summary>
    ///Instantiate une copie du gameobject
    /// </summary>
    public static GameObject Duplicate(this GameObject original)
    {
        return GameObject.Instantiate(original.gameObject);
    }

    #region Destroy gameobject
    public static void Destroy(this GameObject obj, float delay)
    {
        GameObject.Destroy(obj, delay);
    }
    public static void Destroy(this GameObject obj)
    {
        GameObject.Destroy(obj);
    }
    public static void DestroyImmediate(this GameObject obj)
    {
        GameObject.DestroyImmediate(obj);
    }
    public static void LateDestroy(this GameObject go, MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(go.LateDestroyCoroutine());
    }
    public static void LateDestroyImmediate(this GameObject go, MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(go.LateDestroyImmediateCoroutine());
    }
    #endregion

    #region Destroy Component's gameobject
    public static void DestroyGO(this Component obj)
    {
        GameObject.Destroy(obj.gameObject);
    }
    public static void DestroyGO(this Component obj, float delay)
    {
        GameObject.Destroy(obj.gameObject, delay);
    }
    public static void DestroyGOImmediate(this Component obj)
    {
        GameObject.DestroyImmediate(obj.gameObject);
    }
    public static void LateDestroyGO(this Component component, MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(component.gameObject.LateDestroyCoroutine());
    }
    public static void LateDestroyImmediateGO(this Component component, MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(component.gameObject.LateDestroyImmediateCoroutine());
    }
    #endregion


    private static IEnumerator LateDestroyCoroutine(this GameObject go)
    {
        yield return new WaitForEndOfFrame();
        Destroy(go);
    }
    private static IEnumerator LateDestroyImmediateCoroutine(this GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
        Debug.Log("destroy");
    }
}
