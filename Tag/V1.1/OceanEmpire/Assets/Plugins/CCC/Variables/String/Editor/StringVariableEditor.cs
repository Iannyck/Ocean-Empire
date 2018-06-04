using UnityEditor;

[CustomEditor(typeof(StringVariable))]
public class StringVariableEditor : VarVariableEditor<string>
{
    protected override string DrawRuntimeValueField()
    {
        return EditorGUILayout.TextField("Value", variable.Value);
    }
}

