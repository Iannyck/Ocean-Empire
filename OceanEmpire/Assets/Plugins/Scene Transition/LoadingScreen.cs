using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen
{
    private class Wish
    {
        public SceneMessage message;
        public string sceneName;
    }
    private const string SCENENAME = "LoadingScreen";
    private static Wish wish;
    public static bool IsInTransition { get { return isInTransition; } }
    private static bool isInTransition = false;
    private static LoadingScreenAnimation animator;
    private static bool waitForNextSceneSetup = false;

    public static void TransitionTo(string sceneName, SceneMessage message, bool waitForNextSceneSetup = false)
    {
        if (isInTransition)
        {
            throw new System.Exception("Cannot transition to " + sceneName + ". We are already transitioning.");
        }

        LoadingScreen.waitForNextSceneSetup = waitForNextSceneSetup;
        isInTransition = true;

        wish = new Wish()
        {
            message = message,
            sceneName = sceneName
        };
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, OnLoadingScreenLoaded);
    }
    public static void OnNewSetupComplete()
    {
        if (!isInTransition)
            return;

        animator.Outro(OnOutroComplete);
    }

    private static void OnLoadingScreenLoaded(Scene scene)
    {
        animator = Scenes.FindRootObject<LoadingScreenAnimation>(scene);
        animator.Intro(OnIntroComplete);
    }

    private static void OnIntroComplete()
    {
        //Unload all past scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name != SCENENAME)
                Scenes.UnloadAsync(SceneManager.GetSceneAt(i).name);
        }
        DelayManager.LocalCallTo(LateLoad, 0, animator, true);
    }

    private static void LateLoad()
    {
        Scenes.LoadAsync(wish.sceneName, LoadSceneMode.Additive, OnDestinationLoaded);
    }

    private static void OnDestinationLoaded(Scene scene)
    {
        if (!waitForNextSceneSetup)
            OnNewSetupComplete();
        animator.OnNewSceneLoaded();
        if (wish.message != null)
            wish.message.OnLoaded(scene);
    }

    private static void OnOutroComplete()
    {
        Scenes.UnloadAsync(SCENENAME);
        if (wish.message != null)
            wish.message.OnOutroComplete();

        wish = null;
        animator = null;
        isInTransition = false;
    }
}
