using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InfiniteHorizontalScroll : InfiniteScroll
{
    public HorizontalLayoutGroup horizontalLayoutGroup;

    protected override Vector2 GetItemSize()
    {
        RectTransform content = horizontalLayoutGroup.GetComponent<RectTransform>();
        RectTransform child0 = content.GetChild(0) as RectTransform;
        RectTransform child1 = content.GetChild(1) as RectTransform;
        float childSize = (child1.anchoredPosition.x - child0.anchoredPosition.x).Abs() - horizontalLayoutGroup.spacing;

        return new Vector2(childSize, 0);
    }

    protected override Vector2 GetItemSpacing()
    {
        return new Vector2(horizontalLayoutGroup.spacing, 0);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InfiniteHorizontalScroll))]
public class InfiniteHorizontalScrollEditor : InfiniteScrollEditor { }
#endif