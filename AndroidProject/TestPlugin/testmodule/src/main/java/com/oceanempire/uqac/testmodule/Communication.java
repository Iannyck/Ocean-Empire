package com.oceanempire.uqac.testmodule;

import android.util.Log;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

public class Communication {
    public static String DoSthInAndroid()
    {
        Log.i("Unity", "Hi, Sth is done in Android");
        return "WORK";
    }

    public static void SendUnityMessage()
    {
        UnityPlayer.UnitySendMessage("MessageReceiver", "JavaMessage", "HELLO");
    }
}