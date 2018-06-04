using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

public class ExpeSceneChooser : MonoBehaviour
{
    private const string KEY = "firstTimeOpenning";

    void Start()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if (PlayerPrefs.HasKey(KEY))
            {
                Scenes.Load(Shack.SCENENAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
            else
            {
                Scenes.Load(GameBuilder.SCENENAME, UnityEngine.SceneManagement.LoadSceneMode.Single);
                PlayerPrefs.SetInt(KEY, 0);
                PlayerPrefs.Save();
            }
        });
    }
}
