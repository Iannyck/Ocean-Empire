using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static  class ArrayExtensions
{
    public static bool Contains(this Array array, object obj)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array.GetValue(i).Equals(obj))
                return true;
        }
        return false;
    }
}
