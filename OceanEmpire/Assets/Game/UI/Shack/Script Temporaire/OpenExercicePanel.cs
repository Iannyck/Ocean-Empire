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
        InstantExerciseChoice.ProposeTasks(-1);
    }

    void OnSceneLoaded(Scene scene)
    {
        //...
    }
}
