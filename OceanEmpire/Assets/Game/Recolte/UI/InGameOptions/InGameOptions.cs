using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class InGameOptions : WindowAnimation, SceneMessage
{
    public const string SCENENAME = "InGameOptions";

    private bool isQuitting = false;

    private GameBuilder gameBuilder;

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
            LoadingScreen.TransitionTo(GameBuilder.SCENENAME,this,false);
    }

    public void BackToShack()
    {
        LoadingScreen.TransitionTo(FishingSummary.SCENENAME, new ToFishingSummaryMessage(Game.FishingReport));
    }

    public void Exit()
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
                    OnQuit();
                }
            );
        }
        else
        {
            Scenes.UnloadAsync(SCENENAME);
            isQuitting = false;
            OnQuit();
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
        OnStartOpen();
    }

    static void OnStartOpen()
    {
        Time.timeScale = 0;
    }

    static void OnQuit()
    {
        Time.timeScale = 1;
    }

    public void OnLoaded(Scene scene)
    {
        gameBuilder = Scenes.FindRootObject<GameBuilder>(scene);
    }

    public void OnOutroComplete()
    {
        gameBuilder.Init(GameBuilder.mapName);
    }
}
