using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

#if UNITY_EDITOR
[CustomEditor(typeof(InfiniteScroll))]
public class InfiniteScrollEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        InfiniteScroll scroller = target as InfiniteScroll;


        Color guiColor = GUI.color;


        if (!scroller.IsDataOk())
            GUI.color = Color.red;

        if (GUILayout.Button("Fetch layout data"))
        {
            scroller.FetchData();
            if (!Application.isPlaying)
                EditorSceneManager.MarkSceneDirty(scroller.gameObject.scene);
        }



        GUI.color = guiColor;
    }
}
#endif