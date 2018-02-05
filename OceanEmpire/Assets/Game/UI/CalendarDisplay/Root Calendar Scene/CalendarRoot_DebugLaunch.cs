using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using UnityEngine.SceneManagement;

public class CalendarRoot_DebugLaunch : MonoBehaviour
{
    private void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(() =>
        {
            if(Scenes.ActiveSceneCount == 1)
            {
                Scenes.Load(CalendarRootScene.SCENENAME, LoadSceneMode.Additive);
            }
        });
    }
}
