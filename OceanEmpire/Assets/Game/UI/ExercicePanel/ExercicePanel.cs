using CCC.Manager;
using CCC.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExercicePanel : WindowAnimation
{
    public const string SCENENAME = "ExercicePanel";

    public void Quit()
    {
        Close(() => Scenes.UnloadAsync(SCENENAME));
    }

    public void LaunchExerciceInstantane()
    {
        //A CHANGER
        Scenes.LoadAsync(InstantExerciseChoice.SCENENAME, LoadSceneMode.Additive, delegate (Scene scene)
        {
            scene.FindRootObject<InstantExerciseChoice>().Init(true, Quit);
        });
    }
}