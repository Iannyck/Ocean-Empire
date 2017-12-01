using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using CCC.Utility;

public class WidgetFishPop : MonoBehaviour
{

    public Slider gageMeter;
    public float fullRefillAnimLenght = 2;

    public event SimpleEvent AnimComplete;
    public Text ZoneTimeRemaining;

    [SerializeField]
    private bool autoUpdate = false;

    public bool AutoUpdate
    {
        get { return autoUpdate; }
        set
        {
            autoUpdate = value;

            StopAllCoroutines();

            if (autoUpdate)
                StartCoroutine(UpdateLoop());
            else
                StopAllCoroutines();
        }
    }

    void Start()
    {
        UpdateMeter();

        if (autoUpdate)
            StartCoroutine(UpdateLoop());
    }

    IEnumerator UpdateLoop()
    {
        while (true)
        {
            UpdateMeter();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    public void UpdateMeter()
    {
        FishPopulation.instance.RefreshPopulation();
        gageMeter.value = FishPopulation.FishDensity;

        if (ZoneTimeRemaining != null)
            DisplayTimeRemaing();
    }

    public void DisplayTimeRemaing()
    {
        if (ZoneTimeRemaining == null)
            return;

        if (FishPopulation.PopulationRate == 1)
        {
            ZoneTimeRemaining.text = "La population est au maximum!";
            return;
        }

        TimeSpan timeSpan = FishPopulation.GetTimeToRefill();
        string timeString = PrintTime.ShortString(timeSpan);
        ZoneTimeRemaining.text = "<size=34>Compl\u00E8tement rempli dans</size>\n" + timeString;
    }



    public void IncrementRate(float rateDifference, bool andApplyOnFishPop = true)
    {
        float currentRate = FishPopulation.PopulationRate;
        float targetRate = (currentRate + rateDifference).Clamped(0.0f, 1.0f);

        float targetDensity = FishPopulation.GetFishDensityFromRate(targetRate);
        Tweener refillAnim = gageMeter.DOValue(targetDensity, fullRefillAnimLenght).SetUpdate(true);

        if (andApplyOnFishPop)
            FishPopulation.instance.AddRate(rateDifference);

        refillAnim.OnComplete(() =>
        {
            if (AnimComplete != null)
            {
                AnimComplete.Invoke();
                AnimComplete = null;
            }
        });


    }


    public void Fill(Action callBack)
    {
        Tweener refillAnim = gageMeter.DOValue(1, fullRefillAnimLenght).SetUpdate(true);
        FishPopulation.instance.AddRate(1);
        if (callBack != null)
            refillAnim.OnComplete(() => callBack());
    }

    public void DecrementRate(float rateDifference)
    {
        IncrementRate(-rateDifference);

    }

}
