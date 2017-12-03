using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class AudioPlayable : ScriptableObject
{
    public abstract void PlayOn(AudioSource audioSource);
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioPlayable), true)]
public class AudioPlayableEditor: Editor
{
    [SerializeField] private AudioSource _previewer;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioPlayable playable = target as AudioPlayable;

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
        GUILayout.BeginHorizontal();

        GUI.enabled = _previewer.isPlaying;
        if (GUILayout.Button("Stop"))
        {
            _previewer.Stop();
        }
        GUI.enabled = true;

        if (GUILayout.Button("Preview"))
        {
            playable.PlayOn(_previewer);
        }

        GUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
    }

    public void OnEnable()
    {
        _previewer = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    public void OnDisable()
    {
        DestroyImmediate(_previewer.gameObject);
    }
}
#endif