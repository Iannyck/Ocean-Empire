using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatsWindow : WindowAnimation
{ 
    public const string SCENENAME = "CheatsWindow";

    public static void OpenWindow()
    {
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
    }
}
