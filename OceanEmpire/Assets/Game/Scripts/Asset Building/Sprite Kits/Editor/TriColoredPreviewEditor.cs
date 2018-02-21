using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class TriColoredPreviewEditor : Editor
{
    private PreviewRenderUtility _previewRenderUtility;

    protected List<RenderedSprite> renderedObjects = new List<RenderedSprite>();

    private Vector2 _drag;
    private float _distance = DEFAULT_DISTANCE;

    private const float DEFAULT_DISTANCE = 5;
    private const float MOUSE_SENSITIVITY = 0.01f;
    private const float NEAR_CLIP_PLANE = 1;
    private const float FAR_CLIP_PLANE = 100;
    private const float MAX_DISTANCE = FAR_CLIP_PLANE - 0.5f;
    private const float MIN_DISTANCE = NEAR_CLIP_PLANE + 0.5f;

    protected class RenderedSprite
    {
        public TriColoredSprite triColoredSprite;
        public Mesh mesh;
        public Material material;
        public MaterialPropertyBlock propertyBlock;
        public Matrix4x4 matrix;
        public Vector3 position = Vector3.zero;
        public Vector3 baseScale = Vector3.one;
        public Quaternion rotation = Quaternion.identity;

        public void ValidateData()
        {
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
        public void UpdatePropertyBlock()
        {
            if (triColoredSprite == null || triColoredSprite.sprite == null)
                return;

            TriColored.ApplyToPropertyBlock(propertyBlock,
                triColoredSprite.sprite.texture,
                triColoredSprite.colorR,
                triColoredSprite.colorG,
                triColoredSprite.colorB);
        }
        public void UpdateMatrix()
        {
            if (triColoredSprite != null && triColoredSprite.sprite != null)
            {
                var rect = triColoredSprite.sprite.rect;
                var ratio = rect.width / rect.height;

                var scaleFactor = Vector3.one;
                if (ratio > 1)
                    scaleFactor = new Vector3(1, 1 / ratio, 1);
                else
                    scaleFactor = new Vector3(ratio, 1, 1);

                scaleFactor.Scale(baseScale);
                matrix = Matrix4x4.TRS(position, rotation, scaleFactor);
            }
        }
    }
    protected virtual void Awake()
    {
        NewTriColoredSprites();
    }

    protected virtual void OnDestroy()
    {
        if (_previewRenderUtility != null)
            _previewRenderUtility.Cleanup();
    }

    protected virtual void ValidateData()
    {
        if (_previewRenderUtility == null)
        {
            _previewRenderUtility = new PreviewRenderUtility();

            _distance = DEFAULT_DISTANCE;
            _previewRenderUtility.camera.transform.position = new Vector3(0, 0, -_distance);
            _previewRenderUtility.camera.transform.rotation = Quaternion.identity;
            _previewRenderUtility.camera.nearClipPlane = NEAR_CLIP_PLANE;
            _previewRenderUtility.camera.farClipPlane = FAR_CLIP_PLANE;
        }

        for (int i = 0; i < renderedObjects.Count; i++)
        {
            renderedObjects[i].ValidateData();
        }
    }

    public override bool HasPreviewGUI()
    {
        ValidateData();

        return true;
    }


    protected void DrawPreviewButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("Preview"))
        {
            NewTriColoredSprites();
        }
    }

    protected void NewTriColoredSprites()
    {
        ValidateData();

        OnNewColoredSprites();

        for (int i = 0; i < renderedObjects.Count; i++)
        {
            renderedObjects[i].UpdateMatrix();
        }
    }

    protected abstract void OnNewColoredSprites();

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        for (int i = 0; i < renderedObjects.Count; i++)
        {
            renderedObjects[i].UpdatePropertyBlock();
        }

        _drag = Drag2D(_drag, r);

        if (Event.current.type == EventType.Repaint)
        {
            _previewRenderUtility.BeginPreview(r, background);

            for (int i = 0; i < renderedObjects.Count; i++)
            {
                var obj = renderedObjects[i];
                if (obj.mesh == null || obj.material == null || obj.propertyBlock == null)
                    continue;
                _previewRenderUtility.DrawMesh(obj.mesh, obj.matrix, obj.material, 0, obj.propertyBlock);
            }

            _previewRenderUtility.camera.transform.position = Vector2.zero;
            _previewRenderUtility.camera.transform.rotation = Quaternion.Euler(new Vector3(-_drag.y, -_drag.x, 0));
            _previewRenderUtility.camera.transform.position = _previewRenderUtility.camera.transform.forward * -_distance;
            _previewRenderUtility.camera.Render();

            Texture resultRender = _previewRenderUtility.EndPreview();
            GUI.DrawTexture(r, resultRender, ScaleMode.StretchToFill, false);
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
