using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    public static Vector2 GetAnchoredSizeDelta(this RectTransform rect)
    {
        RectTransform parent = (rect.parent as RectTransform);
        if (parent == null)
            return rect.sizeDelta;

        Vector2 parentDelta = parent.GetAnchoredSizeDelta();
        Vector2 myDelta = rect.sizeDelta;
        Vector2 anchorDelta = rect.anchorMax - rect.anchorMin;

        return myDelta + (parentDelta.Scaled(anchorDelta));
        
    }
}
