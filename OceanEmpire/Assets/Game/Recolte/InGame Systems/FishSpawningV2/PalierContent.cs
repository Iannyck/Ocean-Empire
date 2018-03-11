using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PalierContent
{
    public int test = -1;
    public int Index { get; private set; }
    public List<PalierSubscriber> Subscribers { get; private set; }
    public bool IsActive { get; private set; }

    public PalierContent(int index)
    {
        test = index;
        Index = index;
        Subscribers = new List<PalierSubscriber>();
    }

    public void Subscribe(PalierSubscriber subscriber)
    {
        if (subscriber == null)
        {
            Debug.LogError("Null palier subscriber");
            return;
        }
        if (Subscribers.Contains(subscriber))
        {
            Debug.LogError(" The PalierSubscriber (" + subscriber.name + ") tried to subscribe more than once in the same palier");
            return;
        }


        Subscribers.Add(subscriber);
    }

    public void Unsubscribe(PalierSubscriber subscriber)
    {
        Subscribers.Remove(subscriber);
    }

    public void Activate()
    {

    }

    public void Deactivate()
    {

    }
}
