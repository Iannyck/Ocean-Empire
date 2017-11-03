using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using System;
using UnityEngine.SceneManagement;

public class MessageReceiver : BaseManager<MessageReceiver> {

    public const string SCENE_NAME = "DebugMessage";

    public override void Init()
    {
        JavaMessage(GetAndroidMessage());
    }

    public void JavaMessage(string message)
    {
        Scenes.LoadAsync(SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene) {
            scene.FindRootObject<DebugMessageAffichage>().SetText(message);
            DelayManager.LocalCallTo(delegate ()
            {
                Scenes.UnloadAsync(SCENE_NAME);
            }, 1, this);
        });
    }

    public static string GetAndroidMessage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass myClass = new AndroidJavaClass("com.UQAC.OceanEmpire.ActivityDetection");
        return myClass.Call<string>("GetCurrentState", new object[] { });
#else
        return "";
#endif
    }
}
