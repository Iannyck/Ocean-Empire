using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Item/Item")]
public class Item : ScriptableObject
{
    public ItemDescription description;

    public string GetItemID()
    {
        return description.GetItemID();
    }

}
