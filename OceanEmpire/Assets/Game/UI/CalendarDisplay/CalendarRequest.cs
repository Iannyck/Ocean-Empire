using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CalendarRequest
{
    public class Settings
    {
        //public LoadSceneMode sceneLoadingMode = LoadSceneMode.Additive;
        //public bool loadSceneOnExitCalendar;
        //public string exitSceneName;
        //public bool unloadOtherScenesAfterEntrance = false;
        public string windowHeaderTitle = "Calendrier";
        public string scheduleButtonText = "Choisir l'heure";
    }
    public class RequestHandle
    {
        public Action<Scene> onWindowLoaded = (x) => { };
        public Action onWindowIntroComplete = () => { };
        public Action<DateTime> onTimePicked = (x) => { };
        public Action onWindowExitStarted = () => { };
        public Action onWindowExitComplete = () => { };

        public CalendarScroll_Controller scrollController;
        public DayInspector dayInspector;

        private bool closing = false;

        public void CloseWindow(bool closeDayInspectorInstantly = true)
        {
            if (closing)
                return;
            closing = true;

            if (closeDayInspectorInstantly)
            {
                if (dayInspector.IsVisible)
                    dayInspector.HideInstant();
                scrollController.ExitCalendar();
            }
            else
            {
                if (dayInspector.IsVisible)
                    dayInspector.Hide(scrollController.ExitCalendar);
                else
                    scrollController.ExitCalendar();
            }
        }

        // Exemple de fonction qui pourrait être ici
        //public void CancelRequest()
        //{

        //}
    }

    private const string CALENDAR_SCENE = "CalendarScroll";
    public static bool IsInRequest { get; private set; }

    public static RequestHandle LaunchRequest(Settings settings)
    {
        if (IsInRequest)
        {
            Debug.LogError("Already in request");
            return null;
        }

        IsInRequest = true;

        RequestHandle requestHandle = new RequestHandle();

        Scenes.LoadAsync(CALENDAR_SCENE, LoadSceneMode.Additive,
            (scene) =>
            {
                if (requestHandle.onWindowLoaded != null)
                    requestHandle.onWindowLoaded(scene);

                var controller = scene.FindRootObject<CalendarScroll_Controller>();
                controller.SetRequestHandle(requestHandle);
                controller.ApplySettings(settings);

                controller.OnExitComplete = () => IsInRequest = false;
            },
            false);

        return requestHandle;
    }
}
