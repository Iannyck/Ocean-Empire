using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections.ObjectModel;

[CustomEditor(typeof(SpriteKitCategory))]
public class SpriteKitCategoryEditor : Editor
{
    private PreviewRenderUtility _previewRenderUtility;

    private Mesh mesh;
    private Material material;
    private MaterialPropertyBlock propertyBlock;

    private Vector2 _drag;
    private float _distance = DEFAULT_DISTANCE;
    private TriColoredSprite _triColoredSprite;
    private Matrix4x4 _matrix = Matrix4x4.identity;

    private GUIStyle myRadioStyle;
    private GUIStyle myUpDownStyle;
    private ReadOnlyCollection<RandomTriColoredSprite> elements;
    private ReadOnlyCollection<bool> actives;
    private SpriteKitCategory spriteKit;

    private const float DEFAULT_DISTANCE = 5;
    private const float MOUSE_SENSITIVITY = 0.01f;
    private const float NEAR_CLIP_PLANE = 1;
    private const float FAR_CLIP_PLANE = 10;
    private const float MAX_DISTANCE = FAR_CLIP_PLANE - 0.5f;
    private const float MIN_DISTANCE = NEAR_CLIP_PLANE + 0.5f;

    private void ValidateData()
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
        if (_previewRenderUtility == null)
        {
            _previewRenderUtility = new PreviewRenderUtility();

            _distance = DEFAULT_DISTANCE;
            _previewRenderUtility.camera.transform.position = new Vector3(0, 0, -_distance);
            _previewRenderUtility.camera.transform.rotation = Quaternion.identity;
            _previewRenderUtility.camera.nearClipPlane = NEAR_CLIP_PLANE;
            _previewRenderUtility.camera.farClipPlane = FAR_CLIP_PLANE;
        }

        if (mesh == null)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
            DestroyImmediate(gameObject);
        }

        if (material == null)
        {
            material = Resources.Load<Material>(TriColored.COMPLETE_MATERIAL_PATH);
            if (material == null)
                Debug.LogError("Need Tricolored Material in Resources folder: " + TriColored.COMPLETE_MATERIAL_PATH);
        }

        if (propertyBlock == null)
        {
            propertyBlock = new MaterialPropertyBlock();
            UpdatePropertyBlock();
        }
    }

    public override bool HasPreviewGUI()
    {
        ValidateData();

        return true;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

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

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Preview"))
        {
            PickNewTriColoredSprite();
        }
    }

    private void PickNewTriColoredSprite()
    {
        if (elements == null)
        {
            ValidateData();
            return;
        }
        if (elements.Count <= 0)
            return;

        _triColoredSprite = spriteKit.Pick();

        if (_triColoredSprite != null && _triColoredSprite.sprite != null)
        {
            var rect = _triColoredSprite.sprite.rect;
            var ratio = rect.width / rect.height;

            if (ratio > 1)
                _matrix = Matrix4x4.Scale(new Vector3(1, 1 / ratio, 1));
            else
                _matrix = Matrix4x4.Scale(new Vector3(ratio, 1, 1));
        }
    }

    private void UpdatePropertyBlock()
    {
        if (_triColoredSprite == null || _triColoredSprite.sprite == null)
            return;

        TriColored.ApplyToPropertyBlock(propertyBlock,
            _triColoredSprite.sprite.texture,
            _triColoredSprite.colorR,
            _triColoredSprite.colorG,
            _triColoredSprite.colorB);
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        UpdatePropertyBlock();

        _drag = Drag2D(_drag, r);

        if (Event.current.type == EventType.Repaint)
        {
            if (material == null)
            {
                EditorGUI.DropShadowLabel(r, "Material not found.");
            }
            else
            {
                _previewRenderUtility.BeginPreview(r, background);

                _previewRenderUtility.DrawMesh(mesh, _matrix, material, 0, propertyBlock);

                _previewRenderUtility.camera.transform.position = Vector2.zero;
                _previewRenderUtility.camera.transform.rotation = Quaternion.Euler(new Vector3(-_drag.y, -_drag.x, 0));
                _previewRenderUtility.camera.transform.position = _previewRenderUtility.camera.transform.forward * -_distance;
                _previewRenderUtility.camera.Render();

                Texture resultRender = _previewRenderUtility.EndPreview();
                GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);
            }
        }
        if (Event.current.type == EventType.ScrollWheel)
        {
            _distance *= 1 + (Event.current.delta.y * MOUSE_SENSITIVITY);
            _distance = _distance.Clamped(MIN_DISTANCE, MAX_DISTANCE);
            Repaint();
        }
    }

    public override void OnPreviewSettings()
    {
        if (GUILayout.Button("Reset Camera", EditorStyles.whiteMiniLabel))
        {
            _drag = Vector2.zero;
            _distance = DEFAULT_DISTANCE;
        }
    }

    void OnDestroy()
    {
        _previewRenderUtility.Cleanup();
    }

    public static Vector2 Drag2D(Vector2 scrollPosition, Rect position)
    {
        int controlID = GUIUtility.GetControlID("Slider".GetHashCode(), FocusType.Passive);
        Event current = Event.current;
        switch (current.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                if (position.Contains(current.mousePosition) && position.width > 50f)
                {
                    GUIUtility.hotControl = controlID;
                    current.Use();
                    EditorGUIUtility.SetWantsMouseJumping(1);
                }
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    GUIUtility.hotControl = 0;
                }
                EditorGUIUtility.SetWantsMouseJumping(0);
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    scrollPosition -= current.delta * (float)((!current.shift) ? 1 : 3) / Mathf.Min(position.width, position.height) * 140f;
                    scrollPosition.y = Mathf.Clamp(scrollPosition.y, -90f, 90f);
                    current.Use();
                    GUI.changed = true;
                }
                break;
        }
        return scrollPosition;
    }
}
