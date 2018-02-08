using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static int RemoveNulls<T>(this List<T> list)
    {
        return list.RemoveAll((x) => x == null);
    }
    public static void RemoveLast<T>(this List<T> list)
    {
        int index = list.LastIndex();
        if (index >= 0)
        {
            list.RemoveAt(index);
        }
    }
    public static T Last<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }
    public static int LastIndex<T>(this List<T> list)
    {
        return list.Count - 1;
    }
    public static int CountOf<T>(this List<T> list, T element)
    {
        int amount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(element))
                amount++;
        }
        return amount;
    }

    public static T PickRandom<T>(this List<T> list)
    {
        if (list.Count == 0)
            return default(T);
        if (list.Count == 1)
            return list[0];
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static void Shuffle<T>(this List<T> list)
    {
        int chosen = -1;
        T temp;
        for (int i = list.Count - 1; i >= 1; i--)
        {
            chosen = UnityEngine.Random.Range(0, i + 1);
            if (chosen == i)
                continue;

            temp = list[chosen];
            list[chosen] = list[i];
            list[i] = temp;
        }
    }
}
