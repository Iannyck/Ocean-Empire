using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System;

[CustomEditor(typeof(AudioMixerSaver))]
public class AudioMixerSavesEditor : Editor
{
    SerializedProperty _fileName;
    SerializedProperty _mixer;
    SerializedProperty _loadOnEnable;
    private GUIStyle runtimeStyle;
    private AudioMixerSaver audioMixerSaves;
    AudioMixerSaver.ChannelType[] channelTypes;

    void CheckResources()
    {
        if (_fileName == null)
            _fileName = serializedObject.FindProperty("fileName");
        if (_mixer == null)
            _mixer = serializedObject.FindProperty("mixer");
        if (_loadOnEnable == null)
            _loadOnEnable = serializedObject.FindProperty("loadOnInit");

        if (runtimeStyle == null)
        {
            runtimeStyle = new GUIStyle(EditorStyles.boldLabel);
            runtimeStyle.normal.textColor = new Color(0.65f, 0f, 0f);
        }
        audioMixerSaves = (AudioMixerSaver)target;

        var values = Enum.GetValues(typeof(AudioMixerSaver.ChannelType));

        channelTypes = new AudioMixerSaver.ChannelType[values.Length];
        values.CopyTo(channelTypes, 0);
    }

    public override void OnInspectorGUI()
    {
        CheckResources();

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("Open Explorer At Location"))
        {
            string path = Application.persistentDataPath.Replace('/', '\\');

            if (Directory.Exists(path))
            {
                Process.Start("explorer.exe", path);
            }
        }
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_fileName, true);
        EditorGUILayout.LabelField(".dat", GUILayout.Width(45));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(_loadOnEnable, true);
        EditorGUILayout.PropertyField(_mixer, true);


        EditorGUILayout.Space();
        if (GUILayout.Button("Revert to defaults"))
        {
            audioMixerSaves.SetDefaults();
        }
        if (GUILayout.Button("Save to disk"))
        {
            audioMixerSaves.Save();
        }
        if (GUILayout.Button("Load from disk"))
        {
            audioMixerSaves.Load();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("DATA", runtimeStyle);

        for (int i = 0; i < channelTypes.Length; i++)
        {
            DrawChannel(channelTypes[i].ToString(), channelTypes[i]);
        }
    }

    private void DrawChannel(string label, ref AudioMixerSaver.ChannelData channel)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(label);
        channel.muted = EditorGUILayout.Toggle("Muted", channel.muted);
        channel.dbBoost = EditorGUILayout.FloatField("Db Boost", channel.dbBoost);
    }

    private void DrawChannel(string label, AudioMixerSaver.ChannelType channelType)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(label);


        EditorGUI.BeginChangeCheck();

        var muted = EditorGUILayout.Toggle("Muted", audioMixerSaves.GetMuted(channelType));
        var volume = EditorGUILayout.FloatField("Db Boost", audioMixerSaves.GetVolume(channelType));


        if (EditorGUI.EndChangeCheck())
        {
            audioMixerSaves.SetMuted(channelType, muted);
            audioMixerSaves.SetVolume(channelType, volume);
        }
    }
}
