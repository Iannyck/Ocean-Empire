using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Item/Description/Gaz Tank Description")]
public class GazTankDescription : UpgradeDescription
{
    private const string gazTankFolderName = "GazTank";

    override public string GetFolderPath()
    {
        return base.GetFolderPath() + "/" + gazTankFolderName;
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
