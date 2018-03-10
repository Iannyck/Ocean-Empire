using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPComponents;

public class PendingFishGPC
{
    public List<IGPComponent> List { get; private set; }

    public PendingFishGPC()
    {
        List = new List<IGPComponent>();
    }

    public void AddPendingFishGPC(IGPComponent gpc)
    {
        List.Add(gpc);
    }
}
