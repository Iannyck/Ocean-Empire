using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteGridScroll : InfiniteScroll
{
    public RewindEvent onHorizontalRewind = new RewindEvent();
    public RewindEvent onVerticalRewind = new RewindEvent();

    public GridLayoutGroup grid;

    protected override Vector2 GetItemSize()
    {
        return grid.cellSize;
    }

    protected override Vector2 GetItemSpacing()
    {
        return grid.spacing;
    }

    protected override void OnVerticalRewind(int value)
    {
        if (onVerticalRewind != null)
            onVerticalRewind.Invoke(value);
    }

    protected override void OnHorizontalRewind(int value)
    {
        if (onHorizontalRewind != null)
            onHorizontalRewind.Invoke(value);
    }
}