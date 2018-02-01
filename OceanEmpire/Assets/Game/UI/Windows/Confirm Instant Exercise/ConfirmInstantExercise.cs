using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.SceneManagement;
using System;

public class ConfirmInstantExercise : CCC.UI.WindowAnimation
{
    public const string SCENENAME = "ConfirmInstantExercise";

    [Header("Links")]
    public InstantExerciseChoice_Item item;

    private Action<bool> resultCallback;

    /// <summary>
    /// Demande à l'utilisateur de confirmer une tache
    /// </summary>
    public static void OpenWindowAndConfirm(Task task, Action<bool> resultCallback)
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if (Scenes.IsActiveOrBeingLoaded(SCENENAME))
            {
                Scenes.GetActive(SCENENAME).FindRootObject<ConfirmInstantExercise>().Init(task, resultCallback);
            }
            else
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    scene.FindRootObject<ConfirmInstantExercise>().Init(task, resultCallback);
                });
            }
        });
    }

    private void Init(Task task, Action<bool> resultCallback)
    {
        item.DisplayTask(task);
        this.resultCallback = resultCallback;
    }

    public void OnConfirmClick()
    {
        if (resultCallback == null)
            Close();
        else
            Close(() => resultCallback(true));
    }

    public void OnCancelClick()
    {
        if (resultCallback == null)
            Close();
        else
            Close(() => resultCallback(false));
    }
}
