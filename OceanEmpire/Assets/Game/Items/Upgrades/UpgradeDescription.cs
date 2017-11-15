using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UpgradeDescription : ItemDescription
{ 

    private const string upgradeFolderName = "Upgrades";

    override public string GetFolderPath()
    {
        return base.GetFolderPath() + "/" + upgradeFolderName;
    }
}
