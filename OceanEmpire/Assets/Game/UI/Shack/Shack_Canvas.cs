using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shack_Canvas : MonoBehaviour
{
    [Header("MUST BE IN FILTER ORDER")]
    [SerializeField] CanvasGroup[] canvas;
    [SerializeField] float transitionDuration = 0.5f;
    private Tween currentAnim;

    [Flags]
    public enum Filter
    {
        Sections = 1 << 0,
        HUD = 1 << 1,
        AlwaysVisibleHUD = 1 << 2,
        None = 0
    }

    public void HideAllInstant(Filter exculsions = Filter.None)
    {
        Kill();
        SetAlpha(0, exculsions, false);
    }
    public void HideAll(Filter exculsions = Filter.None, TweenCallback onComplete = null)
    {
        Kill();

        float alpha = 1;
        currentAnim = DOTween.To(() => alpha, (x) =>
            {
                SetAlpha(alpha = x, exculsions, false);
            }, 0, transitionDuration).OnComplete(onComplete);
    }

    public void ShowAllInstant(Filter exculsions = Filter.None)
    {
        Kill();
        SetAlpha(1, exculsions, true);
    }
    public void ShowAll(Filter exculsions = Filter.None, TweenCallback onComplete = null)
    {
        Kill();

        float alpha = 0;
        currentAnim = DOTween.To(() => alpha, (x) =>
        {
            SetAlpha(alpha = x, exculsions, true);
        }, 1, transitionDuration).OnComplete(onComplete);
    }

    void SetAlpha(float alpha, Filter exculsions, bool ascending)
    {
        for (int i = 0; i < canvas.Length; i++)
        {
            if ((exculsions & (Filter)(1 << i)) != 0)
                continue;

            canvas[i].alpha = ascending ? Mathf.Max(canvas[i].alpha, alpha) : Mathf.Min(canvas[i].alpha, alpha);
        }
    }

    void Kill()
    {
        if (currentAnim != null && currentAnim.IsActive())
            currentAnim.Kill();
    }

    void OnDestroy()
    {
        Kill();
    }
}
