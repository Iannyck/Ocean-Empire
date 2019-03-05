using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class TaskPanel_Ongoing : MonoBehaviour, ITaskPanelState
{
    public Text timeLimit;
    public Text label;
    public Image sliderBg;
    public Color sliderBgAltColor;
    public Slider slider;
    public Image sliderFill;
    public Sprite sliderFillAltSprite;
    public InstantExerciseChoice_Item taskUI;

    public GameObject feedback;

    private Sprite sliderFillStdSprite;

    private float sliderValue = 0;
    private float sliderValueNow = 0;

    

    public void Enter(Action onComplete)
    {
        gameObject.SetActive(true);

        // Animate
        label.DOFade(0.1f, 0.6f).SetLoops(-1, LoopType.Yoyo);
        sliderBg.DOColor(sliderBgAltColor, 0.6f).SetLoops(-1, LoopType.Yoyo);
        

        if (onComplete != null)
            onComplete();
    }

    public void Exit(Action onComplete)
    {
        gameObject.SetActive(false);

        // Kill animations
        sliderBg.DOKill();
        label.DOKill();

        if (onComplete != null)
            onComplete();
    }

    public void FillContent(object data)
    {
        var report = data as PlannedExerciceRewarder.Report;
        if (report == null)
        {
            Debug.LogError("Could not fill TaskPanel_Planned with data");
            return;
        }

        if(sliderFillStdSprite == null)
            sliderFillStdSprite = sliderFill.sprite;

        taskUI.FillContent(report.schedule.task);
        timeLimit.text = "Heure limite\n" + report.schedule.timeSlot.end.ToCondensedTimeOfDayString(true);
        SetSliderValue(report.GetCompletionRate01());
        sliderValueNow = slider.value;

        if(sliderValue != sliderValueNow)
        {
            ActivateDesactivateFeedBack();
            sliderValue = sliderValueNow;
        }
        else if(feedback.activeSelf){
            ActivateDesactivateFeedBack();
        }
    }

    void AutoUpdateContent()
    {        
        FillContent(PlannedExerciceRewarder.Instance.LatestPendingReport);        
    }

    public void ActivateDesactivateFeedBack()
    {
         
        if (feedback.activeSelf == false)
        {
            feedback.SetActive(true);
        }
        else
        {
            feedback.SetActive(false);
            
        }
    }


    private Tween sliderTween;
    void SetSliderValue(float position01)
    {
        if (sliderTween != null && sliderTween.IsActive())
            sliderTween.Kill();

        position01 = Mathf.Clamp01(position01);

        if (position01 != 1)
        {
            position01 *= 0.95f;
        }

        sliderTween = slider.DOValue(position01, 0.35f).SetEase(Ease.InOutSine);

        if (position01 == 1)
        {
            sliderFill.sprite = sliderFillAltSprite;
            //sliderTween.onComplete = () => sliderFill.sprite = sliderFillAltSprite;
        }
        else
        {
            sliderFill.sprite = sliderFillStdSprite;
        }
    }
}