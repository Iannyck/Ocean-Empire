using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;


public class OpenOptions : MonoBehaviour {
    private void Start()
    {
        MasterManager.Sync();
    }

    public void Open()
    {
        Scenes.LoadAsync(MenuOptions.SCENENAME, LoadSceneMode.Additive, OnSceneLoaded);
    }

    void OnSceneLoaded(Scene scene)
    {
        //...
    }
}
