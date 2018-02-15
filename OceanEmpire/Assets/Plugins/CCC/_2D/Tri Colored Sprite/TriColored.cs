using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode, RequireComponent(typeof(SpriteRenderer))]
public class TriColored : MonoBehaviour
{
    public const string FOLDERNAME = "Materials";
    public const string MATERIALNAME = "MAT_TriColorSprite";
    public const string SHADERNAME = "Ocean Empire/TriColor Sprite";
    public static string COMPLETE_MATERIAL_PATH { get { return FOLDERNAME + "/" + MATERIALNAME; } }

    public Color colorR = new Color(1, 0, 0, 1);
    public Color colorG = new Color(0, 1, 0, 1);
    public Color colorB = new Color(0, 0, 1, 1);

    private MaterialPropertyBlock propertyBlock;
    private SpriteRenderer sprRenderer;

    void Awake()
    {
        Verify();

        if (!Application.isPlaying)
        {
            Material material = Resources.Load(COMPLETE_MATERIAL_PATH) as Material;

#if UNITY_EDITOR
            if (material == null)
            {
                print("null mat");
                Shader shader = Shader.Find(SHADERNAME);
                if (shader == null)
                {
                    Debug.LogError("Besoin du shader " + SHADERNAME);
                    return;
                }
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                if (!AssetDatabase.IsValidFolder("Assets/Resources/" + FOLDERNAME))
                {
                    AssetDatabase.CreateFolder("Assets/Resources", FOLDERNAME);
                }
                Material newMat = new Material(shader);
                AssetDatabase.CreateAsset(newMat, "Assets/Resources/" + FOLDERNAME + "/" + MATERIALNAME + ".mat");
                material = Resources.Load(FOLDERNAME + "/" + MATERIALNAME) as Material;
            }
#endif

            if (sprRenderer != null)
                sprRenderer.sharedMaterial = material;
        }

        Apply();
    }

    private void OnValidate()
    {
        Apply();
    }

    void Verify()
    {
        if (propertyBlock == null)
            propertyBlock = new MaterialPropertyBlock();
        if (sprRenderer == null)
            sprRenderer = GetComponent<SpriteRenderer>();
    }

    public void Apply()
    {
        Verify();
        sprRenderer.GetPropertyBlock(propertyBlock);

        if (sprRenderer.sprite == null)
            return;

        Material mat = sprRenderer.sharedMaterial;
        Texture texture = sprRenderer.sprite.texture;

        ApplyToPropertyBlock(propertyBlock, texture, colorR, colorG, colorB);

        sprRenderer.SetPropertyBlock(propertyBlock);
    }

    public static void ApplyToPropertyBlock(MaterialPropertyBlock block,
        Texture texture, Color colorR, Color colorG, Color colorB)
    {
        block.SetTexture("_MainTex", texture);
        block.SetColor("_ColorR", colorR);
        block.SetColor("_ColorG", colorG);
        block.SetColor("_ColorB", colorB);
    }
}