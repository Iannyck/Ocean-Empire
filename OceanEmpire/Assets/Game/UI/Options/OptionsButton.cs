using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CCC.UI;
using UnityEngine.SceneManagement;

public class OptionsButton : MonoBehaviour
{
    public enum Type { Menu = 0, InGame = 1 }
    public Type type;

    public void OpenOptions()
    {
        switch (type)
        {
            case Type.Menu:
                MenuOptions.Open();
                break;
            case Type.InGame:
                InGameOptions.Open();
                break;
        }
    }
}
