using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InfiniteGridScroll : InfiniteScroll
{
    public GridLayoutGroup grid;

    protected override Vector2 GetItemSize()
    {
        return grid.cellSize;
    }

    protected override Vector2 GetItemSpacing()
    {
        return grid.spacing;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(InfiniteGridScroll))]
public class InfiniteGridScrollEditor : InfiniteScrollEditor { }
#endif