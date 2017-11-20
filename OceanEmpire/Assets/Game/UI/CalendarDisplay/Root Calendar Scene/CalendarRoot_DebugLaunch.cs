using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;

public class CalendarRoot_DebugLaunch : MonoBehaviour
{
    private void Awake()
    {
        MasterManager.Sync(() =>
        {
            if(Scenes.SceneCount() == 2)
            {
                Scenes.Load(CalendarRootScene.SCENENAME, LoadSceneMode.Additive
                    ,(x)=>x.FindRootObject<CalendarRootScene>());
            }
        });
    }
}
