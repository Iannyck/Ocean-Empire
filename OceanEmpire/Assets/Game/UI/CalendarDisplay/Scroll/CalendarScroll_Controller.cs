using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Controller : MonoBehaviour
{
    public const string SCENENAME = "CalendarScroll";

    [Header("Links")]
    public CalendarScroll_Scroller scroller;
    [SerializeField] Button exitCalendarButton;
    [SerializeField] SceneInfo exitScene;
    [SerializeField] CalendarScroll_WindowAnimation windowAnimation;

    [Header("Settings"), SerializeField] int startingDayIndex = 1;

    [ReadOnly, SerializeField] public CalendarRootScene root;

    private void Awake()
    {
        exitCalendarButton.onClick.AddListener(ExitCalendar);
        List<CalendarScroll_Day> days = scroller.days;
        days.ForEach((x) => x.onClick += OnDayClick);

        PersistentLoader.LoadIfNotLoaded(OnCalendarLoaded);
    }

    void OnCalendarLoaded()
    {
        Refill();
        Calendar.instance.OnBonifiedTimeAdded += RefreshContent;
    }

    protected void OnDestroy()
    {
        if (Calendar.instance != null)
            Calendar.instance.OnBonifiedTimeAdded -= RefreshContent;
    }

    public void Show() { windowAnimation.Show(); }
    public void Show(TweenCallback onComplete) { windowAnimation.Show(onComplete); }
    public void Hide() { windowAnimation.Hide(); }
    public bool IsShown { get { return windowAnimation.IsShown; } }

    private void ExitCalendar()
    {
        if (exitScene == null)
        {
            Debug.LogError("No exit scene specified.");
        }
        else
        {
            Scenes.Load(exitScene.SceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive, (scene) =>
            {
                windowAnimation.Hide(root.UnloadAll);
            });
        }
    }

    private void Refill()
    {
        scroller.Fill(Calendar.GetDaysFrom(DateTime.Now.AddDays(-startingDayIndex), scroller.days.Count));
    }
    private void RefreshContent()
    {
        scroller.RefreshContent();
    }

    public void OnDayClick(CalendarScroll_Day day)
    {
        root.dayInspector.ShowAndFill(day.GetDay());
    }

    public void BackToTop()
    {
        Refill();
    }

    public void BackToBottom()
    {
        Refill();
    }
}
