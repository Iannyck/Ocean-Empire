﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CCC.Utility
{
    [System.Serializable]
    public class StatFloat : StatNumber<float>
    {
        public StatFloat(float value) : base(value, float.MinValue, float.MaxValue, BoundMode.Cap) { }
        public StatFloat() : base(0, float.MinValue, float.MaxValue, BoundMode.Cap) { }
        public StatFloat(float value, float min, float max, BoundMode boundMode) : base(value, min, max, boundMode) { }

        protected override float Add(float value, float to)
        {
            return value + to;
        }

        protected override float Divide(float value, float by)
        {
            return value / by;
        }

        protected override float Modulo(float value, float modulo)
        {
            return value % modulo;
        }

        protected override float Multiply(float value, float by)
        {
            return value * by;
        }

        protected override float Substract(float value, float by)
        {
            return value - by;
        }
        
        protected override float OneHundred()
        {
            return 100;
        }

        public static StatFloat operator ++(StatFloat a)
        {
            return a.Set(a.Add(1, a)) as StatFloat;
        }
        public static StatFloat operator --(StatFloat a)
        {
            return a.Set(a.Substract(a, 1)) as StatFloat;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(StatFloat))]
    public class StatFloatDrawer : PropertyDrawer
    {
        float heightByElement = 16;
        int shownElements = 1;
        string newBuffId = "id";
        float newBuffValue = 0;
        BuffType newBuffType = BuffType.Percent;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIStyle bold = new GUIStyle();
            bold.fontStyle = FontStyle.Bold;
            GUIStyle gray = new GUIStyle();
            gray.fontStyle = FontStyle.Italic;

            EditorGUI.BeginChangeCheck();

            StatFloat element = (StatFloat)fieldInfo.GetValue(property.serializedObject.targetObject);
            float x = position.x;
            float y = position.y;
            float width = position.width;

            float _value = property.FindPropertyRelative("value").floatValue;
            float _max = property.FindPropertyRelative("max").floatValue;
            float _min = property.FindPropertyRelative("min").floatValue;
            BoundMode _boundMode = (BoundMode)property.FindPropertyRelative("boundMode").enumValueIndex;


            //Header
            property.isExpanded = EditorGUI.Foldout(new Rect(x, y, width * 0.1f, heightByElement), property.isExpanded, "");
            GUI.color = new Color(1 * IsPlayingVar(), 1 * IsPlayingVar(), 0.9f * IsPlayingVar());
            float newValue = EditorGUI.DelayedFloatField(new Rect(x, y, width, heightByElement), property.displayName, _value);
            shownElements = 1;

            if (newValue != _value)
                element.Set(newValue, false);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel = 1;
                GUI.color = new Color(0.9f * IsPlayingVar(), 1 * IsPlayingVar(), 0.9f * IsPlayingVar());

                //Boundaries
                EditorGUI.LabelField(new Rect(x - 10, GetY(y, shownElements), width, heightByElement), "Boundaries", bold);
                shownElements++;

                //Min, Max                
                float newMin = EditorGUI.DelayedFloatField(new Rect(x, GetY(y, shownElements), width, heightByElement), "Min", _min);
                shownElements++;
                float newMax = EditorGUI.DelayedFloatField(new Rect(x, GetY(y, shownElements), width, heightByElement), "Max", _max);
                shownElements++;

                if (newMin != _min)
                    element.MIN = newMin;
                if (newMax != _max)
                    element.MAX = newMax;

                GUI.color = StdColor();

                //Enum
                element.boundMode = (BoundMode)EditorGUI.EnumPopup(new Rect(x, GetY(y, shownElements), width, heightByElement), "Bound Mode", _boundMode);
                shownElements++;


                //Buffs
                EditorGUI.LabelField(new Rect(x - 10, GetY(y, shownElements), width, heightByElement), "Buffs", bold);
                shownElements++;

                List<StatFloat.Buff> buffList = element.Buffs;
                List<string> buffNames = element.BuffNames;

                int i = 0;
                foreach (StatFloat.Buff buff in buffList)
                {
                    float locY = GetY(y, shownElements);
                    string text = buffNames[i] + ": " + (buff.value >= 0 ? "+" : "") + buff.value + (buff.type == BuffType.Percent ? "%" : "");
                    EditorGUI.LabelField(new Rect(x + 25, locY, width, heightByElement), text);

                    if (GUI.Button(new Rect(x + 15, locY, 16, heightByElement), "X"))
                    {
                        element.RemoveBuff(buffNames[i]);
                    }

                    shownElements++;
                    i++;
                }
                if (i == 0)
                {
                    EditorGUI.LabelField(new Rect(x + 25, GetY(y, shownElements), width, heightByElement), "- no buffs -", gray);
                    shownElements++;
                }

                EditorGUI.LabelField(new Rect(x, GetY(y, shownElements), width, heightByElement), "New Buff");
                float localX = x + 105;
                float localWidth = width - 127;
                float eleWidth = localWidth / 3;
                float padding = 0;
                float localY = GetY(y, shownElements);
                newBuffId = EditorGUI.TextField(new Rect(localX, localY, eleWidth, heightByElement), newBuffId);
                newBuffValue = EditorGUI.FloatField(new Rect(localX + eleWidth + padding, localY, eleWidth, heightByElement), newBuffValue);
                newBuffType = (BuffType)EditorGUI.EnumPopup(new Rect(localX + eleWidth + eleWidth + padding, localY, eleWidth, heightByElement), newBuffType);

                //GUI.enabled = Application.isPlaying;
                if (GUI.Button(new Rect(x + width - 18, localY - 1, 18, heightByElement + 2), "+", EditorStyles.miniButton))
                {
                    element.AddBuff(newBuffId, newBuffValue, newBuffType);
                    newBuffId = "id";
                    newBuffValue = 0;
                    newBuffType = BuffType.Percent;
                }
                shownElements++;
                shownElements++;

                GUI.enabled = true;
            }
            GUI.color = StdColor();

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.SetIsDifferentCacheDirty();
        }

        private Color StdColor()
        {
            float var = IsPlayingVar();
            return new Color(var, var, var);
        }

        private float IsPlayingVar()
        {
            return Application.isPlaying ? 0.796f : 1;
        }

        private float GetY(float y, int shownElements)
        {
            return y + (shownElements * (heightByElement + 2));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return shownElements * heightByElement + (shownElements - 1) * 2;
        }
    }
#endif
}
