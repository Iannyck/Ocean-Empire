using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestScript_Fred : MonoBehaviour
{
    public List<GameObject> toDelete = new List<GameObject>();

    private void Awake()
    {
    }

    void Start()
    {
        Debug.LogWarning("Je suis un test script, ne m'oublie pas (" + gameObject.name + ")");
    }

    private void Update()
    {
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TestScript_Fred))]
public class TestScript_FredEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var t = (TestScript_Fred)target;

        if (GUILayout.Button("get dem bois"))
        {
            t.toDelete.Clear();
            GameObject[] list = FindObjectsOfType<GameObject>();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].hideFlags != HideFlags.None)
                {
                    Debug.Log("name: " + list[i].name + "   type: " + list[i].hideFlags);
                    t.toDelete.Add(list[i]);
                }
            }
        }
        if (GUILayout.Button("delete dem bois"))
        {
            bool setDirty = false;
            for (int i = t.toDelete.Count - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                    Destroy(t.toDelete[i]);
                else
                    DestroyImmediate(t.toDelete[i]);
                setDirty = true;
            }

            if(setDirty)
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
    }
}
#endif