using UnityEditor;


[CustomEditor(typeof(SpriteColors))]
public class SpriteColorsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
            (target as SpriteColors).Apply();
    }
}