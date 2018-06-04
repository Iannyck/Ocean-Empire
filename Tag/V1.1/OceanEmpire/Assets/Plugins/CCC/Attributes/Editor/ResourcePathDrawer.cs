using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ResourcePathAttribute))]
public class ResourcePathDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            Debug.LogError("The [ResourcePath] attribute can only be used on string fields.");
            return;
        }

        Event evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!position.Contains(evt.mousePosition))
                    return;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();

                    if (DragAndDrop.paths.Length > 0)
                    {
                        string path = DragAndDrop.paths[0];

                        // Remove "Asset/"
                        path = path.Remove(0, 7);

                        if (path.Substring(0, 10) == "Resources/")
                        {
                            property.stringValue = path.Remove(0, 10).Split('.')[0];
                        }
                        else
                        {
                            Debug.LogError("The asset must be in the Resources folder");
                        }
                    }
                }
                break;
        }

        var wasGUIColor = GUI.color;
        GUI.color = wasGUIColor * (Resources.Load(property.stringValue) != null ? new Color(0.5f, 1, 0.5f) : new Color(1, 0.5f, 0.5f));

        EditorGUI.PropertyField(position, property, label, true);

        GUI.color = wasGUIColor;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}