using System;

[Serializable]
public class ManualSortedList<T> : BaseSortedList<T>
{
    private Func<T, T, int> comparer;
    public ManualSortedList(Func<T, T, int> comparer) : base()
    {
        this.comparer = comparer;
    }
    public ManualSortedList(Func<T, T, int> comparer, int capacity) : base(capacity)
    {
        this.comparer = comparer;
    }
    protected override int Compare(T a, T b)
    {
        return comparer(a, b);
    }
}