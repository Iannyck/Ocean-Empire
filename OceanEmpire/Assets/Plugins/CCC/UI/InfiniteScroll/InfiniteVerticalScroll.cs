using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteVerticalScroll : InfiniteScroll
{
    public RewindEvent onVerticalRewind = new RewindEvent();

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

    public override void FetchData()
    {
        if (scrollRect.horizontal)
        {
            Debug.LogError("Le ScrollRect ne doit pas avoir Horizontal d'activé.");
            return;
        }

        if (verticalLayoutGroup == null)
        {
            Debug.LogError("Il doit y avoir un VerticalLayoutGroup");
            return;
        }

        RectTransform content = verticalLayoutGroup.GetComponent<RectTransform>();
        if(content.childCount < 2)
        {
            Debug.LogError("Il doit y avoir au moins deux éléments enfants du VerticalLayoutGroup.");
            return;
        }

        base.FetchData();
    }

    public override bool IsDataOk()
    {
        return base.IsDataOk() && !scrollRect.horizontal;
    }

    protected override void OnVerticalRewind(int value)
    {
        if (onVerticalRewind != null)
            onVerticalRewind.Invoke(value);
    }
}