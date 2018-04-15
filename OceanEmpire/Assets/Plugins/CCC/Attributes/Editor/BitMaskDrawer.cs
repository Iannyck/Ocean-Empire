using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomPropertyDrawer(typeof(BitMaskAttribute))]
public class BitMaskPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        // Add the actual int value behind the field name
        label.text = label.text + "(" + prop.intValue + ")";
        prop.intValue = DrawBitMaskField(position, prop.intValue, GetTypeOfProp(prop), label);

        EditorGUI.EndProperty();
    }

    public static int DrawBitMaskField(Rect aPosition, int aMask, System.Type aType, GUIContent aLabel)
    {
        var itemNames = System.Enum.GetNames(aType);
        var itemValues = System.Enum.GetValues(aType) as int[];

        int val = aMask;
        int maskVal = 0;
        for (int i = 0; i < itemValues.Length; i++)
        {
            if (itemValues[i] != 0)
            {
                if ((val & itemValues[i]) == itemValues[i])
                    maskVal |= 1 << i;
            }
            else if (val == 0)
                maskVal |= 1 << i;
        }
        int newMaskVal = EditorGUI.MaskField(aPosition, aLabel, maskVal, itemNames);
        int changes = maskVal ^ newMaskVal;

        for (int i = 0; i < itemValues.Length; i++)
        {
            if ((changes & (1 << i)) != 0)            // has this list item changed?
            {
                if ((newMaskVal & (1 << i)) != 0)     // has it been set?
                {
                    if (itemValues[i] == 0)           // special case: if "0" is set, just set the val to 0
                    {
                        val = 0;
                        break;
                    }
                    else
                        val |= itemValues[i];
                }
                else                                  // it has been reset
                {
                    val &= ~itemValues[i];
                }
            }
        }
        return val;
    }

    public static Type GetTypeOfProp(SerializedProperty property)
    {
        Type parentType = property.serializedObject.targetObject.GetType();
        FieldInfo fi = GetFieldViaPath(parentType, property.propertyPath);
        return fi.FieldType;
    }

    public static FieldInfo GetFieldViaPath(Type type, string path)
    {
        Type parentType = type;
        FieldInfo fi = type.GetField(path);
        string[] perDot = path.Split('.');
        foreach (string fieldName in perDot)
        {
            fi = parentType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi != null)
                parentType = fi.FieldType;
            else
                return null;
        }
        if (fi != null)
            return fi;
        else return null;
    }
}