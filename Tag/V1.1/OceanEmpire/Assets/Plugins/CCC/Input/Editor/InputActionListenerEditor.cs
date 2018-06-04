using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(InputActionListener))]
public class InputActionListenerEditor : Editor
{
    private SerializedProperty _onPress;
    private SerializedProperty _onHold;
    private SerializedProperty _onRelease;
    private InputActionListener _target;

    private void OnEnable()
    {
        _onPress = serializedObject.FindProperty("onPress");
        _onHold = serializedObject.FindProperty("onHold");
        _onRelease = serializedObject.FindProperty("onRelease");
        _target = target as InputActionListener;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();

        if (_target.onPressEvents)
        {
            EditorGUILayout.PropertyField(_onPress);
        }
        if (_target.onHoldEvents)
        {
            EditorGUILayout.PropertyField(_onHold);
        }
        if (_target.onReleaseEvents)
        {
            EditorGUILayout.PropertyField(_onRelease);
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
