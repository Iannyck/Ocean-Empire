using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeight
{
    float Weight { get; }
}

public class Lottery<T>
{
    List<IWeight> list;
    float totalWeight;

    private class Item : IWeight
    {
        public float weight;
        public T obj;
        public float Weight { get { return weight; } set { weight = value; } }
        public Item(T obj, float weight) { this.weight = weight; this.obj = obj; }
    }

    public Lottery(int capacity = 4)
    {
        totalWeight = 0;
        list = new List<IWeight>(capacity);
    }
    public Lottery(IEnumerable<IWeight> c)
    {
        totalWeight = 0;
        foreach (var item in c)
        {
            totalWeight += item.Weight;
        }
        list = new List<IWeight>(c);
    }

    /// <summary>
    /// Nombre d'éléments dans le lot
    /// </summary>
    public int Count
    {
        get
        {
            return list.Count;
        }
    }
    /// <summary>
    /// Le poid total de la lottery
    /// </summary>
    public float TotalWeight
    {
        get
        {
            return totalWeight;
        }
    }

    /// <summary>
    /// Ajout d'un objet dans le lot
    /// </summary>
    public void Add<U>(U obj) where U : IWeight, T
    {
        list.Add(obj);
        totalWeight += obj.Weight;
    }

    /// <summary>
    /// Ajout d'un objet dans le lot en fonction de sa chance d'être sélectionné
    /// </summary>
    public void Add(T obj, float weight)
    {
        list.Add(new Item(obj, weight));
        totalWeight += weight;
    }

    public void AddRange(IEnumerable<IWeight> r)
    {
        foreach (var item in r)
        {
            totalWeight += item.Weight;
        }
        list.AddRange(r);
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= list.Count)
            throw new System.Exception("Tried to remove lottery element out of range.");
        totalWeight -= list[index].Weight;
        list.RemoveAt(index);
    }

    public void Clear()
    {
        list.Clear();
        totalWeight = 0;
    }

    /// <summary>
    /// Sélection d'un élément de facon aléatoire en fonction de leurs chance d'être pigé
    /// </summary>
    public T Pick(out int pickedIndex)
    {
        pickedIndex = -1;

        if (list.Count <= 0)
        {
            Debug.LogError("No lottery item to pick from. Add some before picking.");
            return default(T);
        }

        float ticket = Random.Range(0, totalWeight);
        float currentWeight = 0;
        for (pickedIndex = 0; pickedIndex < list.Count; pickedIndex++)
        {
            currentWeight += list[pickedIndex].Weight;
            if (ticket < currentWeight)
            {
                if (list[pickedIndex] is Item)
                    return (list[pickedIndex] as Item).obj;          //Devrais toujours return ici
                else
                    return (T)list[pickedIndex];
            }
        }

        Debug.LogError("Error in lottery.");
        return default(T);
    }

    /// <summary>
    /// Sélection d'un élément de facon aléatoire en fonction de leurs chance d'être pigé
    /// </summary>
    public T Pick()
    {
        int bidon;
        return Pick(out bidon);
    }
}
