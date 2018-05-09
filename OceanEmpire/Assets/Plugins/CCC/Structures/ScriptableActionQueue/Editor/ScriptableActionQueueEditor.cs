using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableActionQueue))]
public class ScriptableActionQueueEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var src = target as ScriptableActionQueue;

        EditorGUILayout.LabelField("Pending actions: " + src.ActionQueue.PendingCount);
        bool ongoing = src.ActionQueue.IsAnActionOngoing;

        EditorGUILayout.LabelField("Ongoing action: " + ongoing);
        if (ongoing)
        {
            object tg = src.ActionQueue.OngoingTarget;
            if (tg != null)
            {
                string label = "";
                if (tg is Object)
                    label = ((Object)tg).name;
                else
                    label = tg.ToString();

                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField("Target: " + label);
                EditorGUI.indentLevel--;
            }
        }
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }
}
