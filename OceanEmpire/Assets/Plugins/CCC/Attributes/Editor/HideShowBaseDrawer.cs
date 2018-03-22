using System;
using System.Collections;
using System.Collections.Generic;
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
        return IsTrue(property) == IsShownIfMemberTrue ? EditorGUI.GetPropertyHeight(property, label) : -2;
    }

    protected bool IsTrue(SerializedProperty property)
    {
        SerializedObject so = property.serializedObject;

        if (so.isEditingMultipleObjects)
            return DefaultMemberValue;

        object instance = so.targetObject;

        string parentPath = property.propertyPath;
        if (parentPath.Contains("."))
            parentPath = parentPath.Remove(parentPath.LastIndexOf('.'));
        else
            parentPath = "";

        if (parentPath == "")
        {
            // Nous somme déjà au top
            instance = so.targetObject;
        }
        else
        {
            // Nous devons allez creux
            instance = GetSubObjectFromPath(so.targetObject, parentPath);
            if (instance == null)
                return DefaultMemberValue;
        }

        Type type = instance.GetType();
        HideShowBaseAttribute.Type memberType = GetMemberType();
        string name = GetMemberName();

        while (type != null)
        {
            switch (memberType)
            {
                case HideShowBaseAttribute.Type.Property:
                    PropertyInfo propInfo = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo == null)
                        break;
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
                    MethodInfo methodInfo = type.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (methodInfo == null)
                        break;
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
                    FieldInfo fieldInfo = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                    if (fieldInfo == null)
                        break;
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
            type = type.BaseType;
        }

        switch (memberType)
        {
            case HideShowBaseAttribute.Type.Field:
                Debug.LogWarning("(Show/Hide Attribute) Failed to get the field named \"" + name + "\".");
                break;
            case HideShowBaseAttribute.Type.Property:
                Debug.LogWarning("(Show/Hide Attribute) Failed to get the property named \"" + name + "\".");
                break;
            case HideShowBaseAttribute.Type.Method:
                Debug.LogWarning("(Show/Hide Attribute) Failed to get the method named \"" + name + "\".");
                break;
        }
        return DefaultMemberValue;
    }

    object GetSubObjectFromPath(object parentObject, string objectPath)
    {
        string[] fieldNames = objectPath.Split('.');
        Type type = parentObject.GetType();
        FieldInfo fieldInfo = null;

        try
        {
            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (fieldNames[i] == "Array")
                {
                    i++;
                    string dataIndex = fieldNames[i].Substring(5, fieldNames[i].Length - 6);
                    int index = int.Parse(dataIndex);
                    if (parentObject is IList)
                    {
                        var list= (IList)parentObject;
                        if (list.Count <= index)
                            return null;
                        parentObject = list[index];
                    }
                    i++;
                }
                else
                {
                    fieldInfo = type.GetField(fieldNames[i], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    parentObject = fieldInfo.GetValue(parentObject);
                    type = parentObject.GetType();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Error in Hide/Show: " + e.Message);
            return null;
        }

        return parentObject;
    }
}
