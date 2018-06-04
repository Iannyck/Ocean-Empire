using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public abstract class BaseSortedList<T>
{
    private List<T> list;

    public BaseSortedList()
    {
        list = new List<T>();
    }

    public BaseSortedList(int capacity)
    {
        list = new List<T>(capacity);
    }

    public void Add(T item)
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

            int result = Compare(item, list[index]);
            if (result == 0)
            {
                //Parfait !
                list.Insert(index, item);
                return;
            }
            else if (result < 0)
            {
                //Doit etre inséré avant
                count = Mathf.FloorToInt(count / 2f);
            }
            else
            {
                //Doit etre inséré apres
                count = Mathf.CeilToInt(count / 2f) - 1;
                bottom = index + 1;
            }
        }
    }

    public bool Remove(T item)
    {
        return list.Remove(item);
    }
    public void RemoveAt(int index)
    {
        list.RemoveAt(index);
    }

    public void Clear()
    {
        list.Clear();
    }

    public T this[int key]
    {
        get
        {
            return list[key];
        }
    }

    protected abstract int Compare(T a, T b);

    public int Count { get { return list.Count; } }

    public ReadOnlyCollection<T> GetInternalList() { return list.AsReadOnly(); }
}