using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Item/Thruster Description")]
public class ThrusterDescription : UpgradeDescription
{
    private const string thrusterFolderName = "Thrusters";

    override public string GetFolderPath()
    {
        return base.GetFolderPath() + "/" + thrusterFolderName;
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
