using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OkButton : MonoBehaviour
{
    [SerializeField] Vector2 topPosition;
    [SerializeField] Vector2 middlePosition;
    [SerializeField] Vector2 bottomPosition;
    [SerializeField] float fadeInDuration;
    [SerializeField] CanvasGroup visuals;
    [SerializeField] Button button;

    public void SetTopPosition() { GetComponent<RectTransform>().anchoredPosition = topPosition; }
    public void SetMiddlePosition() { GetComponent<RectTransform>().anchoredPosition = middlePosition; }
    public void SetBottomPosition() { GetComponent<RectTransform>().anchoredPosition = bottomPosition; }

    Action onClick;

    void Awake()
    {
        button.onClick.AddListener(OnClick);
        HideInstant();
    }

    void OnClick()
    {
        if (onClick != null)
        {
            onClick();
            onClick = null;
        }

        Hide(0.1f);
    }

    public void PromptOk(float delay, Action onPlayerClick)
    {
        this.DelayedCall(() => PromptOk(onPlayerClick), delay, true);
    }

    public void PromptOk(Action onPlayerClick)
    {
        Show();
        onClick = onPlayerClick;
    }


    Tween currentAnim;

    void Show()
    {
        Kill();
        visuals.blocksRaycasts = true;
        currentAnim = visuals.DOFade(1, fadeInDuration).SetUpdate(true);
    }

    void Hide(float delay)
    {
        Kill();
        visuals.blocksRaycasts = false;
        currentAnim = visuals.DOFade(0, fadeInDuration).SetUpdate(true);
        if (delay > 0)
            currentAnim.SetDelay(delay);
    }

    void HideInstant()
    {
        Kill();
        visuals.alpha = 0;
        visuals.blocksRaycasts = false;
    }

    void Kill()
    {
        if (currentAnim != null && currentAnim.IsActive())
            currentAnim.Kill();
    }
}
