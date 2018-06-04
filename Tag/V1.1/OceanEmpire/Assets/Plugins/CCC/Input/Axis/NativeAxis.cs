using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCC.Input.Axis
{
    [System.Serializable]
    public class NativeAxis : BaseAxis
    {
        public enum NativeType
        {
            Mouse_Vertical = 0,
            Mouse_Horizontal = 1,
            Mouse_Wheel = 2,
            Joystick_Left_Horizontal = 3,
            Joystick_Left_Vertical = 4,
            Joystick_Right_Horizontal = 5,
            Joystick_Right_Vertical = 6
        }

#if UNITY_EDITOR
        [SerializeField] private NativeType type;
#endif

        [SerializeField] private string axisName;
        [SerializeField] private bool useCustomAxis;

        public bool useRawValue;

        protected override float _GetRawValue()
        {
            return useRawValue ? UnityEngine.Input.GetAxisRaw(axisName) : UnityEngine.Input.GetAxis(axisName);
        }

        public NativeAxis(NativeType axisType, bool useRawValue = true, float sensitivity = 1) : base(sensitivity)
        {
            this.useRawValue = useRawValue;
            SetNativeAxis(axisType);
        }

        public NativeAxis(string axisName, bool useRawValue = true, float sensitivity = 1) : base(sensitivity)
        {
            this.useRawValue = useRawValue;
            SetCustomAxis(axisName);
        }

        public string GetAxisName()
        {
            return axisName;
        }
        public bool IsUsingCustomAxis()
        {
            return useCustomAxis;
        }

        public void SetNativeAxis(NativeType type)
        {
#if UNITY_EDITOR
            this.type = type;
#endif
            axisName = NativeTypeToAxisName(type);
            useCustomAxis = false;
        }
        public void SetCustomAxis(string axisName)
        {
            this.axisName = axisName;
            useCustomAxis = true;
        }

        public static string NativeTypeToAxisName(NativeType type)
        {
            switch (type)
            {
                default:
                case NativeType.Mouse_Vertical:
                    return "mv";
                case NativeType.Mouse_Horizontal:
                    return "mh";
                case NativeType.Mouse_Wheel:
                    return "mw";
                case NativeType.Joystick_Left_Horizontal:
                    return "jlh";
                case NativeType.Joystick_Left_Vertical:
                    return "jlv";
                case NativeType.Joystick_Right_Horizontal:
                    return "jrh";
                case NativeType.Joystick_Right_Vertical:
                    return "jrv";
            }
        }
        public static string NativeTypeToDisplayName(NativeType type)
        {
            switch (type)
            {
                default:
                case NativeType.Mouse_Vertical:
                    return "Mouse Vertical";
                case NativeType.Mouse_Horizontal:
                    return "Mouve Horizontal";
                case NativeType.Mouse_Wheel:
                    return "Mouse Wheel";
                case NativeType.Joystick_Left_Horizontal:
                    return "Left Joystick Horizontal";
                case NativeType.Joystick_Left_Vertical:
                    return "Left Joystick Vertical";
                case NativeType.Joystick_Right_Horizontal:
                    return "Right Joystick Horizontal";
                case NativeType.Joystick_Right_Vertical:
                    return "Right Joystick Vertical";
            }
        }

#if UNITY_EDITOR
        public string GetEditorLabelName()
        {
            if (useCustomAxis)
            {
                return "\"" + axisName + "\"";
            }
            else
            {
                return NativeTypeToDisplayName(type);
            }
        }
#endif
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(NativeAxis))]
    public class NativeAxisDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            float height = 18;
            property.isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, height), property.isExpanded, label, true);

            // Calculate rects
            if (property.isExpanded)
            {
                var indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = indent + 1;

                var sensitivityRect = new Rect(position.x, position.y + height, position.width, 18);
                height += 18;
                var useRawValueRect = new Rect(position.x, position.y + height, position.width, 18);
                height += 18;

                EditorGUI.PropertyField(sensitivityRect, property.FindPropertyRelative("sensitivity"));
                EditorGUI.PropertyField(useRawValueRect, property.FindPropertyRelative("useRawValue"));

                if (property.FindPropertyRelative("useCustomAxis").boolValue)
                {
                    var axisNameRect = new Rect(position.x, position.y + height, position.width, 18);
                    EditorGUI.PropertyField(axisNameRect, property.FindPropertyRelative("axisName"));
                    height += 18;
                }

                // Set indent back to what it was
                EditorGUI.indentLevel = indent;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return 18;

            if (property.FindPropertyRelative("useCustomAxis").boolValue)
                return 18 * 4;
            else
                return 18 * 3;
        }
    }
#endif
}
