
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;


public class OpenCheats : MonoBehaviour
{
    private void Start()
    {
        MasterManager.Sync();
    }

    public void Open()
    {
        Scenes.LoadAsync(CheatsWindow.SCENENAME, LoadSceneMode.Additive, OnSceneLoaded);
    }

    void OnSceneLoaded(Scene scene)
    {
        //...
    }
}
