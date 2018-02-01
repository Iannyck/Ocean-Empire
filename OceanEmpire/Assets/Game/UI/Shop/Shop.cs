 
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : WindowAnimation
{
    public const string SCENENAME = "Shop";

    protected override void Start()
    {
        base.Start();
        PersistentLoader.LoadIfNotLoaded();
    }
    public void Quit()
    {
        Close(() => Scenes.UnloadAsync(SCENENAME));
    }
}
