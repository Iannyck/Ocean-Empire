using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

public class Item : BaseBehavior
{
    private void Start()
    {
        
    }
    public ItemDescription description;

    [SerializeField, ReadOnly]
    private string itemID;
    public string GetItemID()
    {
        return itemID;
    }
}
