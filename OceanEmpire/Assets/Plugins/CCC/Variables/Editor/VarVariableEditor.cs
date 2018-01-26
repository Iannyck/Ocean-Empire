using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class VarVariableEditor<T> : Editor
{
    protected VarVariable<T> variable;
    private GUIStyle runtimeStyle;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (variable == null)
            variable = target as VarVariable<T>;

        if (runtimeStyle == null)
        {
            runtimeStyle = new GUIStyle(EditorStyles.boldLabel);
            runtimeStyle.normal.textColor = new Color(0.65f, 0f, 0f);
        }


        if (Application.isPlaying && variable != null)
        {
            var guiColor = GUI.color;

            Color newColor = new Color(guiColor.r, guiColor.g * 0.7f, guiColor.b * 0.7f);
            GUI.color = newColor;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("RUNTIME", runtimeStyle);

            EditorGUI.BeginChangeCheck();
            T newVal = DrawRuntimeValueField();
            if (EditorGUI.EndChangeCheck())
            {
                variable.Value = newVal;
            }

            GUI.color = guiColor;
        }
    }

    public override bool RequiresConstantRepaint()
    {
        return Application.isPlaying;
    }

    protected abstract T DrawRuntimeValueField();
}
