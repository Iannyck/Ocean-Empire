using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : WindowAnimation
{ 
    public const string SCENENAME = "MenuOptions";

    private bool isQuitting = false;

    public void Confirm()
    {
        if (isQuitting)
            return;
        SoundManager.Save();
        Exit();
    }

    public void Cancel()
    {
        if (isQuitting)
            return;
        SoundManager.Load();
        Exit();
    }

    private void Exit()
    {
        if (isQuitting) return;

        isQuitting = true;

        if (this != null)
        {
            Close(
                delegate ()
                {
                    Scenes.UnloadAsync(SCENENAME);
                    isQuitting = false;
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAME);
            isQuitting = false;
        }
    }

    public static void OpenWindow()
    {
        if (Game.instance != null)
        {
            Debug.LogWarning("Cannot open MenuOptions if the game is running.");
            return;
        }

        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
    }
}
