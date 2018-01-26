using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FullInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ClickAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float downSize = 0.93f;
    [Range(0, 10)]
    public float pressSpeed = 4;
    [Range(0, 10)]
    public float releaseSpeed = 0.175f;
    public Ease pressEase = Ease.InSine;
    public Ease releaseEase = Ease.OutElastic;

    private RectTransform tr;
    private bool clickSequence = false;
    private Tween currentTween = null;

    void Start()
    {
        if (GetComponent<RectTransform>() == null)
        {
            Debug.LogError("Cannot put UI_ClickAnimation on a non-UI gameObject");
            enabled = false;
            return;
        }

        tr = GetComponent<RectTransform>();
    }

    public void ManualClickAnim()
    {
        clickSequence = true;
        DownAnim().OnComplete(delegate ()
        {
            clickSequence = false;
            UpAnim();
        });
    }

    private Tween DownAnim()
    {
        currentTween.Kill();
        return currentTween = tr.DOScale(Vector3.one * downSize, 0.1f / (pressSpeed + 0.001f)).SetEase(pressEase);
    }

    private Tween UpAnim()
    {
        currentTween.Kill();
        return currentTween = tr.DOScale(Vector3.one, 0.1f / (releaseSpeed + 0.001f)).SetEase(releaseEase);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickSequence = true;
        DownAnim();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (clickSequence)
        {
            clickSequence = false;
            UpAnim();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (clickSequence)
        {
            clickSequence = false;
            UpAnim();
        }
    }
}