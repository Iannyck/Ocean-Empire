using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class ArrayExtensions
{
    public static T Last<T>(this T[] list)
    {
        return list[list.Length - 1];
    }
    public static int LastIndex(this Array list)
    {
        return list.Length - 1;
    }
    public static int CountOf<T>(this T[] list, T element)
    {
        int amount = 0;
        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].Equals(element))
                amount++;
        }
        return amount;
    }

    public static T PickRandom<T>(this T[] list)
    {
        if (list.Length == 0)
            return default(T);
        if (list.Length == 1)
            return list[0];
        return list[UnityEngine.Random.Range(0, list.Length)];
    }

    public static void Shuffle<T>(this T[] list)
    {
        int chosen = -1;
        T temp;
        for (int i = list.Length - 1; i >= 1; i--)
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
