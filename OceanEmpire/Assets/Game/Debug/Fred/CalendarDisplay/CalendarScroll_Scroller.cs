using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarScroll_Scroller : MonoBehaviour
{
    public float scrollDuration = 0.4f;
    public Ease scrollEase = Ease.InOutSine;
    public ScrollRect scroller;

    [Header("List")]
    public List<CalendarScroll_Week> weeks = new List<CalendarScroll_Week>();

    public bool IsAnimating() { return tween != null && tween.IsActive(); }

    private Tween tween;
    public void ScrollUp(TweenCallback onComplete)
    {
        if (IsAnimating())
            return;

        KillTween();
        tween = scroller.DOVerticalNormalizedPos(1, scrollDuration).SetEase(scrollEase);
        if (onComplete != null)
            tween.OnComplete(onComplete);
    }
    public void ScrollDown(TweenCallback onComplete)
    {
        if (IsAnimating())
            return;

        KillTween();
        tween = scroller.DOVerticalNormalizedPos(0, scrollDuration).SetEase(scrollEase);
        if (onComplete != null)
            tween.OnComplete(onComplete);
    }

    public void CenterScroller()
    {
        KillTween();

        scroller.verticalNormalizedPosition = 0.5f;
    }

    public CalendarScroll_Week PutTopAtBottom()
    {
        CalendarScroll_Week transitioningWeek = weeks[0];
        weeks.RemoveAt(0);
        weeks.Add(transitioningWeek);
        transitioningWeek.transform.SetAsLastSibling();

        return transitioningWeek;
    }

    public CalendarScroll_Week PutBottomAtTop()
    {
        CalendarScroll_Week transitioningWeek = weeks.Last();
        weeks.RemoveLast();
        weeks.Insert(0, transitioningWeek);
        transitioningWeek.transform.SetAsFirstSibling();
        return transitioningWeek;
    }

    void KillTween()
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
            tween = null;
        }
    }
}
