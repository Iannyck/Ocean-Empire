using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FullInspector;

[CreateAssetMenu(menuName = "Ocean Empire/Item")]
public class Item : ScriptableObject
{
    public ItemDescription description;

    public string GetItemID()
    {
        return description.GetItemID();
    }

}
