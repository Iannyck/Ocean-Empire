
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class InGameOptions : WindowAnimation
{
    public const string SCENENAME = "InGameOptions";

    private bool isQuitting = false;

    public void Confirm()
    {
        Exit();
    }

    public void Cancel()
    {
        Exit();
    }

    public void RestartGame()
    {
        if (Game.Instance != null)
        {
            // NB: On fait ça pour prévenir la game de finir d'une toute autre façon
            Game.Instance.gameOver = true;
            UnlockTime();

            var sceneMessage = new ToRecolteMessage(Game.Instance.GameSettings);
            LoadingScreen.TransitionTo(GameBuilder.SCENENAME, sceneMessage, true);
        }
    }

    public void BackToShack()
    {
        Exit(() => Game.Instance.EndGame());
    }

    public void Exit(Action onComplete = null)
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
                    UnlockTime();
                    if (onComplete != null)
                        onComplete();
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAME);
            isQuitting = false;
            UnlockTime();
            if (onComplete != null)
                onComplete();
        }
    }

    public static void OpenWindow()
    {
        if (Game.Instance == null)
        {
            Debug.LogWarning("Cannot open InGameOptions if the game is not running.");
            return;
        }

        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive);
        Locktime();
    }

    static void Locktime()
    {
        Game.Instance.GameRunning.Lock("option");
    }

    static void UnlockTime()
    {
        Game.Instance.GameRunning.Unlock("option");
    }
}
