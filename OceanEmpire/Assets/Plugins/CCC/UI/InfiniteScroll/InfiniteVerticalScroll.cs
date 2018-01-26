using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteVerticalScroll : InfiniteScroll
{
    public VerticalLayoutGroup verticalLayoutGroup;

    protected override Vector2 GetItemSize()
    {
        RectTransform content = verticalLayoutGroup.GetComponent<RectTransform>();
        RectTransform child0 = content.GetChild(0) as RectTransform;
        RectTransform child1 = content.GetChild(1) as RectTransform;
        float childSize = (child1.anchoredPosition.y - child0.anchoredPosition.y).Abs() - verticalLayoutGroup.spacing;

        return new Vector2(0, childSize);
    }

    protected override Vector2 GetItemSpacing()
    {
        return new Vector2(0, verticalLayoutGroup.spacing);
    }
}