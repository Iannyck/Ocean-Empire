using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Ocean Empire/Item/Description/Harpoon Thrower Description")]
public class HarpoonThrowerDescription : UpgradeDescription
{
    private const string HarpoonFolderName = "Harpoon";

    override public string GetFolderPath()
    {
        return base.GetFolderPath() + "/" + HarpoonFolderName;
    }
    /*
    override public T GetItem<T>()
    {
        string path = GetItemPath();
        T thing = Instantiate(Resources.Load(path)) as T;
        return thing;
    }
    */
}

