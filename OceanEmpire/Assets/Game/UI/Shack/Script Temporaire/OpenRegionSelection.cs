using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.SceneManagement;

public class OpenRegionSelection : MonoBehaviour
{
    private void Start()
    {
        PersistentLoader.LoadIfNotLoaded();
    }

    public void Open()
    {
        Scenes.LoadAsync(RegionSelection.SCENENAME, LoadSceneMode.Additive, OnSceneLoaded);
    }

    void OnSceneLoaded(Scene scene)
    {
        //...
    }
}
