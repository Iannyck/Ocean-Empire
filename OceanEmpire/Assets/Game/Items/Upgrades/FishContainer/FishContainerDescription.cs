using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Ocean Empire/Item/Description/Fish Container Description")]
public class FishContainerDescription : UpgradeDescription
{
    private const string fishContainerFolderName = "FishContainer";

    override public string GetFolderPath()
    {
        return base.GetFolderPath() + "/" + fishContainerFolderName;
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

