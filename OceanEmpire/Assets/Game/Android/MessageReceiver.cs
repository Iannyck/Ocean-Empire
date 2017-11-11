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
        CompleteInit();
    }

    public void JavaMessage(string message)
    {
        Scenes.LoadAsync(SCENE_NAME, LoadSceneMode.Additive, delegate (Scene scene) {
            scene.FindRootObject<DebugMessageAffichage>().SetText(message);
            DelayManager.LocalCallTo(delegate ()
            {
                Scenes.UnloadAsync(SCENE_NAME);
            }, 5, this);
        });
    }

    public string GetAndroidMessage()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        var ajc = new AndroidJavaClass("com.oceanempire.uqac.testmodule.Communication"); //(1)
        ajc.CallStatic<string>("DoSthInAndroid");                                                //(2)

        return "WORK";
#else
        return "";
#endif
    }
}
