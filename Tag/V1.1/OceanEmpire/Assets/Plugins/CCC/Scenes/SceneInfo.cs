using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
#endif

[CreateAssetMenu(fileName = "SI_NewScene", menuName = "CCC/Scenes/Scene Info")]
public class SceneInfo : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset scene;
    public SceneAsset Editor_GetScene()
    {
        return scene;
    }
    public void Editor_RefreshSceneName()
    {
        sceneName = scene.name;
    }
#endif

    [SerializeField, ReadOnly]
    private string sceneName;

    [SerializeField, Header("Defaults")] private LoadSceneMode loadMode = LoadSceneMode.Additive;
    [SerializeField] private bool allowMultiple = false;

    public string SceneName { get { return sceneName; } }
    public bool AllowMultiple { get { return allowMultiple; } }
    public LoadSceneMode LoadMode { get { return loadMode; } }

#if UNITY_EDITOR
    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceID);
        SceneInfo sceneInfo = obj as SceneInfo;
        if (sceneInfo != null)
        {
            if (sceneInfo.Editor_GetScene() != null)
            {
                if (!Application.isPlaying)
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetOrScenePath(sceneInfo.Editor_GetScene()));
                return true;
            }
        }

        return false; // we did not handle the open
    }
#endif
}
