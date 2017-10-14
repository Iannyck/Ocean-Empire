using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSelection : WindowAnimation
{
    public const string SCENENAME = "RegionSelection";

    public void Quit()
    {
        Close(() => Scenes.UnloadAsync(SCENENAME));
    }
}
