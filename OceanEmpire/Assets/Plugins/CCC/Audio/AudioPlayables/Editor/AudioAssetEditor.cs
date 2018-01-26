using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioPlayable), true)]
public class AudioPlayableEditor : Editor
{
    [SerializeField] private AudioSource _previewer;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioPlayable playable = target as AudioPlayable;

        EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

        GUILayoutOption largeHeight = GUILayout.Height(18 * 2 + 3);

        GUILayout.BeginHorizontal(largeHeight);

        GUI.enabled = _previewer.isPlaying;
        if (GUILayout.Button("Stop", largeHeight))
        {
            _previewer.Stop();
        }
        GUI.enabled = true;

        GUILayout.BeginVertical();
        if (GUILayout.Button("Preview"))
        {
            playable.PlayOnAndIgnoreCooldown(_previewer);
        }
        if (GUILayout.Button("Preview looped"))
        {
            if (_previewer.isPlaying)
                _previewer.Stop();
            playable.PlayLoopedOn(_previewer);
        }
        GUILayout.EndVertical();

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