using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Item/ShopMap")]
public class ShopMapDescription : ItemDescription
{
    [SerializeField]
    private MapDescription mapDescription;

    private const string upgradeFolderName = "Shop Map";


    override public string GetFolderPath()
    {
        return base.GetFolderPath() + "/" + upgradeFolderName;
    }

    override public string GetName()
    {
        return mapDescription.GetName();
    }

    override public string GetDescription()
    {
        return mapDescription.GetDescription();
    }

    override public Sprite GetImage()
    {
        return mapDescription.GetImage();
    }

    public MapDescription GetMapDescription()
    {
        return mapDescription;
    }
}
