using UnityEditor;

[CustomEditor(typeof(BoolVariable))]
public class BoolVariableEditor : VarVariableEditor<bool>
{
    protected override bool DrawRuntimeValueField()
    {
        return EditorGUILayout.Toggle("Value", variable.Value);
    }
}
