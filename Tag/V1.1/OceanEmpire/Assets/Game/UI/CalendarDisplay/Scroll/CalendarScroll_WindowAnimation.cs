using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CalendarScroll_WindowAnimation : MonoBehaviour
{
    [Header("Components"), SerializeField] RectTransform window;

    [Header("General Settings"), SerializeField] bool showOnStart;

    [Header("Show"), SerializeField] Vector2 shownAnchoredPos; 
    [SerializeField] Ease showEase = Ease.OutQuint;
    [SerializeField] float showDuration = 0.4f;

    [Header("Hide"), SerializeField] Vector2 hiddenAnchoredPos;
    [SerializeField] Ease hideEase = Ease.OutQuint;
    [SerializeField] float hideDuration = 0.35f;

    private Tween currentAnim;
    private bool isShown = false;
    public bool IsShown { get { return isShown; } }

    void Awake()
    {
        HideInstant();
    }

    void Start()
    {
        if (showOnStart)
            Show();
    }

    public void ShowInstant()
    {
        Kill();
        window.anchoredPosition = shownAnchoredPos;
        isShown = true;
    }
    public void HideInstant()
    {
        Kill();
        window.anchoredPosition = hiddenAnchoredPos;
        isShown = false;
    }

    public void Show() { Show(null); }
    public void Show(TweenCallback onComplete)
    {
        Kill();

        isShown = true;
        currentAnim = window.DOAnchorPos(shownAnchoredPos, showDuration)
            .SetEase(showEase)
            .OnComplete(onComplete);
    }

    public void Hide() { Hide(null); }
    public void Hide(TweenCallback onComplete)
    {
        Kill();

        isShown = false;
        currentAnim = window.DOAnchorPos(hiddenAnchoredPos, hideDuration)
            .SetEase(hideEase)
            .OnComplete(onComplete);
    }

    private void Kill()
    {
        if (currentAnim != null)
        {
            if (currentAnim.IsActive())
            {
                currentAnim.Kill();
            }
            currentAnim = null;
        }
    }
}
