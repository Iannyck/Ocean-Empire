using UnityEditor;

[CustomEditor(typeof(FloatVariable))]
public class FloatVariableEditor : VarVariableEditor<float>
{
    protected override float DrawRuntimeValueField()
    {
        return EditorGUILayout.FloatField("Current Value", variable.Value);
    }
}
