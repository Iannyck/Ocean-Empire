using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CalendarScroll_Controller : MonoBehaviour
{
    public const string SCENENAME = "CalendarScroll";

    [Header("Scenes"), SerializeField] SceneInfo dayInspectorScene;
    [SerializeField] SceneInfo exitScene;

    [Header("Components"), SerializeField] CalendarScroll_Scroller scroller;
    [SerializeField] Button exitCalendarButton;
    [SerializeField] CalendarScroll_WindowAnimation windowAnimation;
    [SerializeField] Image clickBlocker;

    [Header("Settings"), SerializeField] int startingDayIndex = 1;

    private DayInspector dayInspector;

    private bool everythingIsLoaded = false;
    private bool canAnimateEntrance = false;
    private bool entranceComplete = false;
    private Action onEntranceComplete = null;

    private void Awake()
    {
        PersistentLoader.LoadIfNotLoaded(OnPersistentObjectsLoaded);

        //Add local listeners
        exitCalendarButton.onClick.AddListener(ExitCalendar);
        List<CalendarScroll_Day> days = scroller.days;
        days.ForEach((x) => x.onClick += OnDayClick);
    }

    void Start()
    {
        canAnimateEntrance = true;

        if (everythingIsLoaded)
            AnimateEntrance();
    }

    void OnPersistentObjectsLoaded()
    {
        //Load the DayInspector
        Scenes.Load(dayInspectorScene, OnDayInspectorLoaded);
    }

    void OnDayInspectorLoaded(Scene scene)
    {
        dayInspector = scene.FindRootObject<DayInspector>();

        // Fill data
        Refill();

        //Add listeners
        Calendar.instance.OnBonifiedTimeAdded += RefreshContent;

        everythingIsLoaded = true;

        if (canAnimateEntrance)
            AnimateEntrance();
    }

    private void AnimateEntrance()
    {
        // NOTE: le CallNextFrame est utilisé pour rendre plus fluide l'animation d'entré. 
        //       Si on ne le fait pas, les premières frames de l'animation d'entré sont coupés.
        this.CallNextFrame(() =>
        {
            windowAnimation.Show(() =>
            {
                clickBlocker.enabled = false;
                entranceComplete = true;
                if (onEntranceComplete != null)
                    onEntranceComplete();
            });
        });
    }

    public void OnEntranceComplete(Action callback)
    {
        if (entranceComplete)
            callback();
        else
            onEntranceComplete = callback;
    }

    protected void OnDestroy()
    {
        if (Calendar.instance != null)
            Calendar.instance.OnBonifiedTimeAdded -= RefreshContent;
    }

    private void ExitCalendar()
    {
        if (exitScene == null)
        {
            Debug.LogError("No exit scene specified.");
        }
        else
        {
            Scenes.Load(exitScene.SceneName, LoadSceneMode.Additive, (scene) =>
            {
                windowAnimation.Hide(() =>
                {
                    Scenes.UnloadAsync(dayInspector.gameObject.scene);
                    Scenes.UnloadAsync(gameObject.scene);
                });
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
        dayInspector.ShowAndFill(day.GetDay());
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
