using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UpgradeDescriptionOLD : ItemDescription
{ 

    private const string upgradeFolderName = "Upgrades";
    [SerializeField]
    private int upgradeLevel;

    override public string GetFolderPath()
    {
        return base.GetFolderPath() + "/" + upgradeFolderName;
    }
    public int GetUpgradeLevel()
    {
        return upgradeLevel;
    }
}
