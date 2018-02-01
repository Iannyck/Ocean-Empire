
using UnityEngine;
 
using UnityEngine.SceneManagement;


public class OpenCheats : MonoBehaviour
{
    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
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
