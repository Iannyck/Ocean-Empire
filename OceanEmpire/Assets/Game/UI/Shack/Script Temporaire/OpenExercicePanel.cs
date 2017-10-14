using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenExercicePanel : MonoBehaviour
{
    private void Start()
    {
        MasterManager.Sync();
    }

    public void Open()
    {
        Scenes.LoadAsync(ExercicePanel.SCENENAME, LoadSceneMode.Additive, OnSceneLoaded);
    }

    void OnSceneLoaded(Scene scene)
    {
        //...
    }
}
