using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Palier
{
    public int Index { get; private set; }
    [SerializeField] private List<PalierSubscriber> subscribers;
    public List<PalierSubscriber> Subscribers { get { return subscribers; } private set { subscribers = value; } }
    public bool IsActive { get; private set; }
    public OneWayBool HasBeenSeen;
    public int FishToRespawn { get; set; }

    public Palier(int index)
    {
        IsActive = false;
        HasBeenSeen = new OneWayBool(false);
        Index = index;
        Subscribers = new List<PalierSubscriber>();
        FishToRespawn = 0;
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
        IsActive = true;
        for (int i = Subscribers.Count - 1; i >= 0; i--)
        {
            Subscribers[i].OnPalierActivate();
        }
    }

    public void Deactivate()
    {
        IsActive = false;
        for (int i = Subscribers.Count - 1; i >= 0; i--)
        {
            Subscribers[i].OnPalierDeactivate();
        }
    }
}
