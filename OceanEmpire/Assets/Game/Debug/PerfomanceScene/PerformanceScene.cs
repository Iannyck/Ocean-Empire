using CCC.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceScene : MonoBehaviour
{
    private const string SCENENAME = "PerformanceScene";

    public void GoToPerformanceScene()
    {
        Scenes.Load(SCENENAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    void Awake()
    {
        MasterManager.Sync();
    }

    public void GoBackToShack()
    {
        Scenes.Load(Shack.SCENENAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
