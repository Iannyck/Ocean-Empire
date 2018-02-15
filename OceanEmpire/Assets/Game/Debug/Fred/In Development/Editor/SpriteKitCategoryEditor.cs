using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    private const float DEFAULT_DISTANCE = 5;
    private const float MOUSE_SENSITIVITY = 0.01f;
    private const float NEAR_CLIP_PLANE = 1;
    private const float FAR_CLIP_PLANE = 10;
    private const float MAX_DISTANCE = FAR_CLIP_PLANE - 0.5f;
    private const float MIN_DISTANCE = NEAR_CLIP_PLANE + 0.5f;

    private void ValidateData()
    {
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
            _previewRenderUtility.m_Camera.transform.position = new Vector3(0, 0, -_distance);
            _previewRenderUtility.m_Camera.transform.rotation = Quaternion.identity;
            _previewRenderUtility.m_Camera.nearClipPlane = NEAR_CLIP_PLANE;
            _previewRenderUtility.m_Camera.farClipPlane = FAR_CLIP_PLANE;
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
        base.OnInspectorGUI();

        var array = serializedObject.FindProperty("elements");
        if (array == null)
            return;

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < array.arraySize; i++)
        {
            var element = array.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginHorizontal();
            //Mute
            GUILayout.Toggle(false, GUIContent.none, GUILayout.Width(16));

            //Solo
            GUILayout.Toggle(false, GUIContent.none, myRadioStyle, GUILayout.Width(16));

            GUILayout.Space(7);

            //Property
            EditorGUILayout.PropertyField(element, true, GUILayout.MinWidth(75));

            //Move up
            GUILayout.Button("\u25b2", myUpDownStyle, GUILayout.Width(25), GUILayout.Height(16));
            //Move down
            GUILayout.Button("\u25bc", myUpDownStyle, GUILayout.Width(25), GUILayout.Height(16));

            GUILayout.Space(10);

            //Delete
            GUILayout.Button("X", GUILayout.Width(16), GUILayout.Height(16));
            EditorGUILayout.EndHorizontal();
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Preview"))
        {
            PickNewTriColoredSprite();
        }
    }

    private void PickNewTriColoredSprite()
    {
        SpriteKitCategory spriteKit = (SpriteKitCategory)target;
        if (spriteKit.elements == null)
            return;
        if (spriteKit.elements.Count <= 0)
            return;

        var pick = spriteKit.elements.PickRandom();
        if (pick != null)
            _triColoredSprite = pick.GetRandomTriColoredSprite();

        if (_triColoredSprite.sprite != null)
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

                _previewRenderUtility.m_Camera.transform.position = Vector2.zero;
                _previewRenderUtility.m_Camera.transform.rotation = Quaternion.Euler(new Vector3(-_drag.y, -_drag.x, 0));
                _previewRenderUtility.m_Camera.transform.position = _previewRenderUtility.m_Camera.transform.forward * -_distance;
                _previewRenderUtility.m_Camera.Render();

                Texture resultRender = _previewRenderUtility.EndPreview();
                GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);
            }
        }
        if (Event.current.type == EventType.scrollWheel)
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
