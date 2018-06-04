using UnityEditor;

[CustomEditor(typeof(IntVariable))]
public class IntVariableEditor : VarVariableEditor<int>
{
    protected override int DrawRuntimeValueField()
    {
        return EditorGUILayout.IntField("Value", variable.Value);
    }
}
