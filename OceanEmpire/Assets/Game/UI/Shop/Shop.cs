using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : WindowAnimation
{
    public const string SCENENAME = "Shop";

    private void Start()
    {
        CCC.Manager.MasterManager.Sync();
    }
    public void Quit()
    {
        Close(() => Scenes.UnloadAsync(SCENENAME));
    }
}
