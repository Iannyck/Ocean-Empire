using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections.ObjectModel;

[CustomEditor(typeof(SpriteKitCategory))]
public class SpriteKitCategoryEditor : TriColoredPreviewEditor
{
    private GUIStyle myRadioStyle;
    private GUIStyle myUpDownStyle;
    private ReadOnlyCollection<RandomTriColoredSprite> elements;
    private ReadOnlyCollection<bool> actives;
    private SpriteKitCategory spriteKit;

    protected override void ValidateData()
    {
        if(spriteKit == null)
            spriteKit = (SpriteKitCategory)target;

        if (!spriteKit.VerifyIntegrity())
            spriteKit.Clear();

        if (elements == null)
            elements = spriteKit.GetElements();

        if (actives == null)
            actives = spriteKit.GetActives();

        if (myRadioStyle == null)
        {
            myRadioStyle = new GUIStyle(EditorStyles.radioButton)
            {
                margin = new RectOffset(0, 0, 1, 0),
            };
        }
        if (myUpDownStyle == null)
        {
            myUpDownStyle = new GUIStyle(EditorStyles.miniButton)
            {
                margin = new RectOffset(2, 2, 3, 0)
            };
        }

        base.ValidateData();
    }

    public override void OnInspectorGUI()
    {
        var array = serializedObject.FindProperty("elements");
        if (array == null)
            return;

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < array.arraySize; i++)
        {
            var serializedElement = array.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();
            //Active
            GUI.enabled = !spriteKit.IsSoloEnabled;
            var active = GUILayout.Toggle(actives[i], GUIContent.none, GUILayout.Width(16));
            if (active != actives[i])
                spriteKit.SetElementActive(i, active);
            GUI.enabled = true;

            //Solo
            var soloOn = GUILayout.Toggle(spriteKit.GetSoloElement() == i, GUIContent.none, myRadioStyle, GUILayout.Width(16));
            if (soloOn && spriteKit.GetSoloElement() != i)
                spriteKit.SetSoloElement(i);
            else if (!soloOn && spriteKit.GetSoloElement() == i)
                spriteKit.DisableSoloElement();

            GUILayout.Space(7);

            Rect lastRect = GUILayoutUtility.GetLastRect();
            Rect xRect = new Rect(Screen.width - 7 - 16, lastRect.y, 16, 16);
            Rect downRect = new Rect(xRect.xMin - 25 - 10, lastRect.y, 25, 16);
            Rect upRect = new Rect(downRect.xMin - 25 - 3, lastRect.y, 25, 16);


            //Move up
            if (i > 0 && GUI.Button(upRect, "\u25b2", myUpDownStyle))
            {
                spriteKit.ReorderElements(i, i - 1);
                serializedObject.Update();
            }
            //Move down
            if (i < elements.Count - 1 && GUI.Button(downRect, "\u25bc", myUpDownStyle))
            {
                spriteKit.ReorderElements(i, i + 1);
                serializedObject.Update();
            }

            //Delete
            if (GUI.Button(xRect, "X"))
            {
                spriteKit.RemoveElement(i);
                serializedObject.Update();
                i--;
            }
            else
            {
                var label = new GUIContent(i + ": " + (elements[i].sprite == null ? "null" : elements[i].sprite.name));
                EditorGUILayout.PropertyField(serializedElement, label, true);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add"))
        {
            spriteKit.AddElement(new RandomTriColoredSprite());
            serializedObject.Update();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        DrawPreviewButton();
    }

    protected override TriColoredSprite GetPreviewSprite()
    {
        if (elements == null)
        {
            ValidateData();
            return null;
        }
        if (elements.Count <= 0)
            return null;

        return spriteKit.Pick();
    }
}
