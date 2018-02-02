using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AndroidNotification {

    public class NotificationMessage
    {
        public TimeSpan delay;
        public string title;
        public string message;
        public Color32 color;
        public bool sound;
        public bool vibrate;
        public bool lights;
        public string appIcon;

        public NotificationMessage(string title, string message, Color color, TimeSpan delay, string appIcon)
        {
            this.delay = delay;
            this.title = title;
            this.message = message;
            this.color = GetColor(color);
            sound = true;
            vibrate = true;
            lights = true;
            this.appIcon = appIcon;
        }

        public NotificationMessage(string title, string message, Color color, bool sound, bool vibrate, bool lights, TimeSpan delay, string appIcon)
        {
            this.delay = delay;
            this.title = title;
            this.message = message;
            this.color = GetColor(color);
            this.sound = sound;
            this.vibrate = vibrate;
            this.lights = lights;
            this.appIcon = appIcon;
        }

        public static Color32 GetColor(Color color)
        {
            return new Color32((byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), 255);
        }
    }

    public static void ClearNotifications()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        LocalNotification.ClearNotifications();
        #endif
    }

    public static void CancelNotification(int id)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        LocalNotification.CancelNotification(id);
        #endif
    }

    public static int SendNotification(NotificationMessage message)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return LocalNotification.SendNotification(message.delay, 
                                                  message.title, 
                                                  message.message, 
                                                  message.color, 
                                                  message.sound, 
                                                  message.vibrate, 
                                                  message.lights, 
                                                  message.appIcon);
        #else
        return 0;
        #endif
    }

    public static int SendNotification(int id, NotificationMessage message)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return LocalNotification.SendNotification(id,
                                                  message.delay,
                                                  message.title,
                                                  message.message,
                                                  message.color,
                                                  message.sound,
                                                  message.vibrate,
                                                  message.lights,
                                                  message.appIcon);
        #else
        return 0;
        #endif
    }

    public static int SendNotification(int id, NotificationMessage message, long msDelay)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return LocalNotification.SendNotification(id,
                                                  msDelay,
                                                  message.title,
                                                  message.message,
                                                  message.color,
                                                  message.sound,
                                                  message.vibrate,
                                                  message.lights,
                                                  message.appIcon);
        #else
        return 0;
        #endif
    }

    public static int SendRepeatingNotification(NotificationMessage message, TimeSpan timeOut)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return LocalNotification.SendRepeatingNotification(message.delay,
                                                           timeOut,
                                                           message.title,
                                                           message.message,
                                                           message.color,
                                                           message.sound,
                                                           message.vibrate,
                                                           message.lights,
                                                           message.appIcon);
        #else
        return 0;
        #endif
    }

    public static int SendRepeatingNotification(int id, NotificationMessage message, TimeSpan timeOut)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return LocalNotification.SendRepeatingNotification(id,
                                                   message.delay,
                                                   timeOut,
                                                   message.title,
                                                   message.message,
                                                   message.color,
                                                   message.sound,
                                                   message.vibrate,
                                                   message.lights,
                                                   message.appIcon);
        #else
        return 0;
        #endif
    }

    public static int SendRepeatingNotification(int id, NotificationMessage message, long msDelay, long timeOut)
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        return LocalNotification.SendRepeatingNotification(id,
                                           msDelay,
                                           timeOut,
                                           message.title,
                                           message.message,
                                           message.color,
                                           message.sound,
                                           message.vibrate,
                                           message.lights,
                                           message.appIcon);
        #else
        return 0;
        #endif
    }
}
