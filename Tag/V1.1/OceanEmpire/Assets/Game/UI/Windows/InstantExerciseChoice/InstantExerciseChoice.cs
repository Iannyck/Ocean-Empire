
using System.Collections;
using System.Collections.Generic;
using CCC.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InstantExerciseChoice : MonoBehaviour
{
    public const string SCENENAME = "InstantExercise";

    public InstantExerciseChoice_Item[] taskDisplays;

    public Action<InstantExerciseChoice, Task> ResultCallback { get; set; }

    private bool isClosing = false;

    /// <summary>
    /// Load la scene et propose des taches
    /// </summary>
    public static void ProposeTasks(Action<InstantExerciseChoice, Task> resultCallback)
    {
        InstantExerciseChoice controller = null;
        Action onLoad = () =>
        {
            controller.ResultCallback = resultCallback;
        };

        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if (Scenes.IsActive(SCENENAME))
            {
                controller = Scenes.GetActive(SCENENAME).FindRootObject<InstantExerciseChoice>();
                onLoad();
            }
            else
            {
                Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
                {
                    controller = scene.FindRootObject<InstantExerciseChoice>();
                    onLoad();
                });
            }
        });
    }

    void Awake()
    {
        for (int i = 0; i < taskDisplays.Length; i++)
        {
            taskDisplays[i].onClick = OnItemClick;
        }
    }

    public void Cancel()
    {
        if (ResultCallback != null)
            ResultCallback(this, null);
    }

    private void OnItemClick(InstantExerciseChoice_Item item)
    {
        if (ResultCallback != null)
            ResultCallback(this, item.assignedTask);
    }

    public void CloseWindow()
    {
        if (isClosing)
            return;
        isClosing = true;

        GetComponent<WindowAnimation>().Close(() => Scenes.UnloadAsync(gameObject.scene));
    }
}
