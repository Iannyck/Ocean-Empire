using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

using CCC.UI;
public class ItemDescScene : WindowAnimation
{

    public const string SCENENAME = "ItemDescription";

    public static Action close;



    public static void OpenItemDescription(IShopDisplayable item, Action callback)
    {
        close = callback;
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if (Scenes.IsActive(SCENENAME))
            {
                Scenes.GetActive(SCENENAME).FindRootObject<ItemDisplay2>().SetValues(item);
            }
            else
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    scene.FindRootObject<ItemDisplay2>().SetValues(item);
                });
            }
        });
    }

    public void Quit()
    {
        close();
        Close(() => Scenes.UnloadAsync(SCENENAME));
    }

}

