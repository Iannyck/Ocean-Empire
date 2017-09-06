using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static int RemoveNulls<T>(this List<T> list)
    {
        return list.RemoveAll((x) => x == null);
    }
}
