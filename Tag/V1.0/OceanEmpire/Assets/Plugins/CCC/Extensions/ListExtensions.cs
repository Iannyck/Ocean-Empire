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
        if(index >= 0)
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
    public static void SortedAdd<T>(this List<T> list, T item) where T : IComparable
    {
        int count = list.Count;
        int bottom = 0;
        while (true)
        {
            if (count == 0)
            {
                list.Insert(bottom, item);
                return;
            }

            int index = bottom + Mathf.FloorToInt(count / 2f);

            int result = item.CompareTo(list[index]);
            if (result == 0)
            {
                //Parfait !
                list.Insert(index, item);
                return;
            }
            else if (result < 0)
            {
                //Doit etre ins�r� avant
                count = Mathf.FloorToInt(count / 2f);
            }
            else
            {
                //Doit etre ins�r� apres
                count = Mathf.CeilToInt(count / 2f) - 1;
                bottom = index + 1;
            }
        }
    }
    public static void SortedAdd<T>(this List<T> list, T item, Func<T, T, int> comparer)
    {
        int count = list.Count;
        int bottom = 0;
        while (true)
        {
            if (count == 0)
            {
                list.Insert(bottom, item);
                return;
            }

            int index = bottom + Mathf.FloorToInt(count / 2f);

            int result = comparer(item, list[index]);
            if (result == 0)
            {
                //Parfait !
                list.Insert(index, item);
                return;
            }
            else if (result < 0)
            {
                //Doit etre ins�r� avant
                count = Mathf.FloorToInt(count / 2f);
            }
            else
            {
                //Doit etre ins�r� apres
                count = Mathf.CeilToInt(count / 2f) - 1;
                bottom = index + 1;
            }
        }
    }
}
