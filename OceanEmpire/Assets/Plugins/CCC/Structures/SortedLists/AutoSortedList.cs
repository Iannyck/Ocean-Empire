using System.Collections.Generic;
using System;

[Serializable]
public class AutoSortedList<T> : BaseSortedList<T> where T : IComparable
{
    public AutoSortedList() : base() { }
    public AutoSortedList(int capacity) : base(capacity) { }

    protected override int Compare(T a, T b)
    {
        return a.CompareTo(b);
    }
}
[Serializable]
public class AutoSortedList<T, U> : BaseSortedList<T> where U : IComparer<T>
{
    protected U comparer;
    public U Comparer { get { return comparer; } set { comparer = value; } }

    public AutoSortedList(U comparer) :base()
    {
        this.comparer = comparer;
    }
    public AutoSortedList(U comparer, int capacity) : base(capacity)
    {
        this.comparer = comparer;
    }
    protected override int Compare(T a, T b)
    {
        return comparer.Compare(a, b);
    }
}