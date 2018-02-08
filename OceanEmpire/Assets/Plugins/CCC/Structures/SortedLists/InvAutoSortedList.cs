using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class InvAutoSortedList<T> : BaseSortedList<T> where T : IComparable
{
    public InvAutoSortedList() : base() { }
    public InvAutoSortedList(int capacity) : base(capacity) { }

    protected override int Compare(T a, T b)
    {
        return -a.CompareTo(b);
    }
}
[Serializable]
public class InvAutoSortedList<T, U> : AutoSortedList<T, U> where U : IComparer<T>
{
    public InvAutoSortedList(U comparer) : base(comparer) { }
    public InvAutoSortedList(U comparer, int capacity) : base(comparer, capacity) { }

    protected override int Compare(T a, T b)
    {
        return -base.Compare(a, b);
    }
}
