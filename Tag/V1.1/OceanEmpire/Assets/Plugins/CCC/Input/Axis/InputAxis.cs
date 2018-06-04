using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Input.Axis;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "CCC/Input/Input Axis", fileName = "IA_NewAxis")]
public class InputAxis : ScriptableObject
{
    [Header("Keys")]
    public List<KeyAxis> keyAxes = new List<KeyAxis>();

    [Header("Other")]
    public List<NativeAxis> nativeAxes = new List<NativeAxis>();

    public const string AN_LEFTJOYSTICK_VERTICAL = "ljv";
    public const string AN_LEFTJOYSTICK_HORIZONTAL = "ljh";
    public const string AN_RIGHTJOYSTICK_VERTICAL = "ljv";
    public const string AN_RIGHTJOYSTICK_HORIZONTAL = "ljh";
    public const string AN_MOUSE_VERTICAL = "mv";
    public const string AN_MOUSE_HORIZONTAL = "mh";
    public const string AN_MOUSE_WHEEL = "mw";

    /// <summary>
    /// Retourne la valeur de l'axe, entre -1 et 1.
    /// </summary>
    public float GetValue()
    {
        float value = 0;

        //Check les KeyAxes
        for (int i = 0; i < keyAxes.Count; i++)
        {
            Consider(ref value, keyAxes[i].GetValue());
        }

        //Check les custom axis
        for (int i = 0; i < nativeAxes.Count; i++)
        {
            Consider(ref value, nativeAxes[i].GetValue());
        }

        return value;
    }

    private void Consider(ref float val, float newVal)
    {
        if (newVal.Abs() > val.Abs())
            val = newVal;
    }
}

namespace CCC.Input.Axis
{
#if UNITY_EDITOR
    [CustomEditor(typeof(InputAxis))]
    public class InputAxisEditor : Editor
    {
        SerializedProperty _keyAxes;
        SerializedProperty _nativeAxes;
        InputAxis _axis;

        private void OnEnable()
        {
            _axis = target as InputAxis;
            FetchProperties();
        }

        private void FetchProperties()
        {
            if (_axis.keyAxes == null)
                _axis.keyAxes = new List<KeyAxis>();
            if (_axis.nativeAxes == null)
                _axis.nativeAxes = new List<NativeAxis>();

            _keyAxes = serializedObject.FindProperty("keyAxes");
            _nativeAxes = serializedObject.FindProperty("nativeAxes");
        }

        private void AddAxis(object val)
        {
            int type = (int)val;
            switch (type)
            {
                case -2:
                    _axis.keyAxes.Add(new KeyAxis(KeyCode.None, KeyCode.None));
                    break;
                case -1:
                    _axis.nativeAxes.Add(new NativeAxis("Enter axis name"));
                    break;
                default:
                    _axis.nativeAxes.Add(new NativeAxis((NativeAxis.NativeType)type));
                    break;
            }
            UpdateObject();
        }

        private void UpdateObject()
        {
            EditorUtility.SetDirty(_axis);
            serializedObject.UpdateIfRequiredOrScript();
            FetchProperties();
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("ADD"))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Keycodes"), false, AddAxis, -2);
                menu.AddItem(new GUIContent("Mouse/Horizontal"), false, AddAxis, 1);
                menu.AddItem(new GUIContent("Mouse/Vertical"), false, AddAxis, 0);
                menu.AddItem(new GUIContent("Mouse/Wheel"), false, AddAxis, 2);
                menu.AddItem(new GUIContent("Joystick/Left Horizontal"), false, AddAxis, 3);
                menu.AddItem(new GUIContent("Joystick/Left Vertical"), false, AddAxis, 4);
                menu.AddItem(new GUIContent("Joystick/Right Horizontal"), false, AddAxis, 5);
                menu.AddItem(new GUIContent("Joystick/Right Vertical"), false, AddAxis, 6);
                menu.AddItem(new GUIContent("Custom Axis"), false, AddAxis, -1);
                menu.ShowAsContext();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Key Axes", EditorStyles.boldLabel);


            for (int i = 0; i < _keyAxes.arraySize; i++)
            {
                SerializedProperty child = _keyAxes.GetArrayElementAtIndex(i);

                string label = _axis.keyAxes[i].ToString();
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(child, new GUIContent(label), true);

                if (GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    _axis.keyAxes.RemoveAt(i);
                    UpdateObject();
                    return;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Non-Key Axes", EditorStyles.boldLabel);

            for (int i = 0; i < _nativeAxes.arraySize; i++)
            {
                SerializedProperty child = _nativeAxes.GetArrayElementAtIndex(i);

                string label = _axis.nativeAxes[i].GetEditorLabelName();
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(child, new GUIContent(label), true);

                if (GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    _axis.nativeAxes.RemoveAt(i);
                    UpdateObject();
                    return;
                }

                EditorGUILayout.EndHorizontal();


                string axisName = _axis.nativeAxes[i].GetAxisName();
                try
                {
                    UnityEngine.Input.GetAxis(axisName);
                }
                catch
                {
                    EditorGUILayout.HelpBox("Vous devez avoir un axe nommé " + axisName + " dans l'input natif de unity.", MessageType.Error);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
