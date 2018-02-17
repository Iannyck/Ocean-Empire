using UnityEngine;
using UnityEditor;
using System.Collections.ObjectModel;

[CustomEditor(typeof(PrebuiltSpriteKit))]
public class PrebuiltSpriteKitEditor : TriColoredPreviewEditor
{
    private PrebuiltSpriteKit spriteKit;

    protected override void ValidateData()
    {
        if (spriteKit == null)
            spriteKit = (PrebuiltSpriteKit)target;

        UpdateObjectCount();
        UpdateObjectPositions();

        base.ValidateData();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawPreviewButton();
    }

    void UpdateObjectCount()
    {
        var length = spriteKit.Length;
        if (renderedObjects.Count == length)
            return;
        if (renderedObjects.Count < length)
        {
            for (int i = renderedObjects.Count; i < length; i++)
            {
                renderedObjects.Add(new RenderedSprite());
            }
        }
        else
        {
            for (int i = length; i < renderedObjects.Count; i++)
            {
                renderedObjects.RemoveAt(i);
            }
        }
    }

    void UpdateObjectPositions()
    {
        var length = renderedObjects.Count;
        var spacing = 1f;
        var mid = length / 2;
        for (int i = 0; i < length; i++)
        {
            var position = new Vector3((i - mid) * spacing, 0, 0);
            renderedObjects[i].position = position;
        }
    }

    protected override void OnNewColoredSprites()
    {
        for (int i = 0; i < renderedObjects.Count; i++)
        {
            renderedObjects[i].triColoredSprite = spriteKit.Get(i);
        }
    }
}
