using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class HideShowBaseDrawer : PropertyDrawer
{
    protected abstract bool DefaultMemberValue { get; }
    protected abstract bool IsShownIfMemberTrue { get; }

    protected string GetMemberName()
    {
        return ((HideShowBaseAttribute)attribute).name;
    }
    protected HideShowBaseAttribute.Type GetMemberType()
    {
        return ((HideShowBaseAttribute)attribute).type;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (IsTrue(property) == IsShownIfMemberTrue)
            EditorGUI.PropertyField(position, property, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return IsTrue(property) == IsShownIfMemberTrue ? base.GetPropertyHeight(property, label) : -2;
    }

    protected bool IsTrue(SerializedProperty property)
    {
        SerializedObject so = property.serializedObject;

        if (so.isEditingMultipleObjects)
            return DefaultMemberValue;

        UnityEngine.Object instance = so.targetObject;
        Type type = instance.GetType();
        HideShowBaseAttribute.Type memberType = GetMemberType();
        string name = GetMemberName();

        switch (memberType)
        {
            case HideShowBaseAttribute.Type.Property:
                PropertyInfo propInfo = type.GetProperty(name);
                if (propInfo == null)
                {
                    Debug.LogWarning("(Show/Hide Attribute) Failed to get the property named \"" + name + "\".");
                    return DefaultMemberValue;
                }
                else if (propInfo.PropertyType != typeof(bool))
                {
                    Debug.LogWarning("(Show/Hide Attribute) The property named \"" + name + "\" is not of type bool.");
                    return DefaultMemberValue;
                }
                else
                {
                    return (bool)propInfo.GetValue(instance, null);
                }
            case HideShowBaseAttribute.Type.Method:
                MethodInfo methodInfo = type.GetMethod(name);
                if (methodInfo == null)
                {
                    Debug.LogWarning("(Show/Hide Attribute) Failed to get the method named \"" + name + "\".");
                    return DefaultMemberValue;
                }
                else if (methodInfo.ReturnType != typeof(bool))
                {
                    Debug.LogWarning("(Show/Hide Attribute) The method named \"" + name + "\" does not return a bool.");
                    return DefaultMemberValue;
                }
                else
                {
                    return (bool)methodInfo.Invoke(instance, null);
                }
            case HideShowBaseAttribute.Type.Field:
                FieldInfo fieldInfo = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (fieldInfo == null)
                {
                    Debug.LogWarning("(Show/Hide Attribute) Failed to get the field named \"" + name + "\".");
                    return DefaultMemberValue;
                }
                else if (fieldInfo.FieldType != typeof(bool))
                {
                    Debug.LogWarning("(Show/Hide Attribute) The field named \"" + name + "\" is not of type bool.");
                    return DefaultMemberValue;
                }
                else
                {
                    return (bool)fieldInfo.GetValue(instance);
                }
            default:
                Debug.LogWarning("Error in Show/Hide property drawing.");
                return DefaultMemberValue;
        }
    }
}
