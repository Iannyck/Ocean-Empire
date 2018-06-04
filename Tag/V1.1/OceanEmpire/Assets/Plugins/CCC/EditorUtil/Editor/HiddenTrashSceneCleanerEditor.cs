using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HiddenTrashSceneCleaner))]
public class HiddenTrashSceneCleanerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var t = target as HiddenTrashSceneCleaner;

        if (GUILayout.Button("Search"))
        {
            List<GameObject> allObjects = new List<GameObject>();
            t.gameObject.scene.GetRootGameObjects(allObjects);

            for (int i = allObjects.Count - 1; i >= 0; i--)
            {
                if ((allObjects[i].hideFlags & HideFlags.HideInHierarchy) == 0)
                {
                    allObjects.RemoveAt(i);
                }
            }

            t.hiddenGameobjects = allObjects.ToArray();
        }
        if (GUILayout.Button("Destroy"))
        {
            foreach (var obj in t.hiddenGameobjects)
            {
                if (Application.isPlaying)
                    Destroy(obj);
                else
                    DestroyImmediate(obj);
            }
        }
    }
}