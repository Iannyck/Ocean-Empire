using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WidgetFishPop : MonoBehaviour {

    public Slider gageMeter;
    private float fullRefillAnimLenght = 2;

    public event SimpleEvent AnimComplete;

    void Start()
    {
        UpdateMeter();
    }

    public void UpdateMeter()
    {
        FishPopulation.instance.RefreshPopulation();
        gageMeter.value = FishPopulation.PopulationRate;
    }


    public void IncrementRate(float rateDifference)
    {
        float currentRate = FishPopulation.PopulationRate;
        float target = (currentRate + rateDifference).Clamped(0.0f, 1.0f);
        float animDelay = (rateDifference * fullRefillAnimLenght).Abs();

        Tweener refillAnim = gageMeter.DOValue(target, animDelay);
                
        refillAnim.OnComplete(() => {
            if (AnimComplete != null)
            {
                AnimComplete.Invoke();
                AnimComplete = null;
            }
        });

        FishPopulation.instance.AddRate(rateDifference);
    }


    public void DecrementRate(float rateDifference)
    {
        IncrementRate(-rateDifference);
    }

        // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            print("inpoute");
            IncrementRate(0.2f);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            IncrementRate(-0.2f);
        }
    }

}
