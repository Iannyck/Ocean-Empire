using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu(menuName = "Ocean Empire/Item Description")]
public class ItemDescription : ScriptableObject
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private string itemDescription;
    [SerializeField]
    private int moneyCost;
    [SerializeField]
    private int ticketCost;
    [SerializeField]
    private Sprite itemImage;

    [SerializeField]
    private string itemFileName;

    private const string itemFolderName = "Items";
    [SerializeField]
    private string itemID;

    virtual public string GetFolderPath(){
        return itemFolderName;
    }

    /*
    virtual public Item GetItem()
    {
        return Instantiate( Resources.Load(GetItemPath()) as Item );
    }*/

    
    virtual public T GetItem<T>() where T: Object
    {
        string path = GetCompletePath();
        Object obj = Resources.Load(path);
        T thing = Instantiate(obj) as T;
        return thing;
    }

    public string GetCompletePath() {
        return GetFolderPath() + "/" + itemFileName;
    }



    public string GetItemID(){
        return itemID;
    }

    public string GetName()  {
        return itemName;
    }

    public string GetDescription(){
        return itemDescription;
    }

    public int GetMoneyCost(){
        return moneyCost;
    }

    public int GetTicketCost(){
        return ticketCost;
    }

    public Sprite GetImage(){
        return itemImage;
    }
}
