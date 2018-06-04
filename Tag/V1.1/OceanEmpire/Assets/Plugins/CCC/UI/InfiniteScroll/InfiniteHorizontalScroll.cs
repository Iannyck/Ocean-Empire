using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteHorizontalScroll : InfiniteScroll
{
    public RewindEvent onHorizontalRewind = new RewindEvent();

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

    public override void FetchData()
    {
        if (scrollRect.vertical)
        {
            Debug.LogError("Le ScrollRect ne doit pas avoir Vertical d'activé.");
            return;
        }

        if (horizontalLayoutGroup == null)
        {
            Debug.LogError("Il doit y avoir un HorizontalLayoutGroup");
            return;
        }

        RectTransform content = horizontalLayoutGroup.GetComponent<RectTransform>();
        if (content.childCount < 2)
        {
            Debug.LogError("Il doit y avoir au moins deux éléments enfants du HorizontalLayoutGroup.");
            return;
        }

        base.FetchData();
    }

    public override bool IsDataOk()
    {
        return base.IsDataOk() && !scrollRect.vertical;
    }

    protected override void OnHorizontalRewind(int value)
    {
        if (onHorizontalRewind != null)
            onHorizontalRewind.Invoke(value);
    }
}