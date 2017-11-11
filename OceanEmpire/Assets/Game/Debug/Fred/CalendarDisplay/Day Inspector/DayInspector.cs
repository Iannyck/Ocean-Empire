using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DayInspector : MonoBehaviour
{
    public const string SCENENAME = "DayInspector";

    [Header("Links")]
    public Button exitButton;
    public RectTransform container;
    public CanvasGroupBehaviour blackBG;

    [Header("Animation")]
    public float moveDuration;
    public Ease hideEase;
    public Ease showEase;
    public Vector2 hiddenAnchoredPos;
    public Vector2 shownAnchoredPos;

    [ReadOnly]
    public CalendarRootScene root;

    private void Awake()
    {
        HideInstant();
        exitButton.onClick.AddListener(Hide);
    }

    public void Show(Calendar.Day day) { Show(day, null); }
    public void Show(Calendar.Day day, TweenCallback onComplete)
    {
        blackBG.Show();

        container.DOKill();
        container.DOAnchorPos(shownAnchoredPos, moveDuration).SetEase(showEase).OnComplete(onComplete);
    }

    public void Hide() { Hide(null); }
    public void Hide(TweenCallback onComplete)
    {
        blackBG.Hide();

        container.DOKill();
        container.DOAnchorPos(hiddenAnchoredPos, moveDuration).SetEase(hideEase).OnComplete(onComplete);
    }

    public void ShowInstant()
    {
        blackBG.ShowInstant();

        container.DOKill();
        container.anchoredPosition = shownAnchoredPos;
    }

    public void HideInstant()
    {
        blackBG.HideInstant();

        container.DOKill();
        container.anchoredPosition = hiddenAnchoredPos;
    }
}
