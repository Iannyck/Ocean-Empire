using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CanvasGroupBehaviour : MonoBehaviour
{
    [Header("Canvas group")]
    public CanvasGroup canvasGroup;

    [Header("Settings"), SerializeField, ReadOnlyInPlayMode]
    private float fadeDuration = 0.35f;
    public Ease ease = Ease.Linear;
    public bool setInteractable;
    public bool setBlocksRaycast;

    public bool isIndependantUpdate = false;

    [SerializeField, ReadOnlyInPlayMode]
    private bool keepTheSameTween = true;

    private Tween keepTween;
    private bool _isShown = true;

    public bool IsShown
    {
        get { return _isShown; }
        private set
        {
            _isShown = value;

            if (setBlocksRaycast)
                canvasGroup.blocksRaycasts = _isShown;
            if (setInteractable)
                canvasGroup.interactable = _isShown;
        }
    }

    public void Hide()
    {
        Hide(null);
    }

    public virtual void Hide(TweenCallback onComplete)
    {
        IsShown = false;

        if (keepTheSameTween)
        {
            BuildKeepTween();
            keepTween.PlayForward();
        }
        else
        {
            KillIfActive();
            CheckLaunchTween(0);
        }
        keepTween.OnComplete(onComplete);
    }

    public void Show()
    {
        Show(null);
    }

    public virtual void Show(TweenCallback onComplete)
    {
        IsShown = true;

        if (keepTheSameTween)
        {
            BuildKeepTween();
            keepTween.PlayBackwards();
        }
        else
        {
            KillIfActive();
            CheckLaunchTween(1);
        }

        keepTween.OnRewind(onComplete);
    }

    public virtual void HideInstant()
    {
        IsShown = false;

        if (keepTheSameTween)
        {
            if (keepTween != null)
            {
                keepTween.PlayForward();
                keepTween.Complete();
            }
            else
            {
                canvasGroup.alpha = 0;
            }
        }
        else
        {
            KillIfActive();
            canvasGroup.alpha = 0;
        }
    }

    public virtual void ShowInstant()
    {
        IsShown = true;

        if (keepTheSameTween)
        {
            if (keepTween != null)
            {
                keepTween.Goto(0);
                keepTween.PlayBackwards();
            }
            else
            {
                canvasGroup.alpha = 1;
            }
        }
        else
        {
            KillIfActive();
            canvasGroup.alpha = 1;
        }
    }

    protected virtual void OnDestroy()
    {
        KillIfActive();
    }

    private void CheckLaunchTween(float alpha)
    {
        if (keepTween == null || !keepTween.IsActive())
            keepTween = AdjustTween(canvasGroup.DOFade(alpha, fadeDuration));
    }

    private void BuildKeepTween()
    {
        if(keepTween == null)
        {
            float a = canvasGroup.alpha;
            canvasGroup.alpha = 1;
            keepTween = AdjustTween(canvasGroup.DOFade(0, fadeDuration));
            keepTween.ForceInit();
            canvasGroup.alpha = a;
        }
    }

    private void KillIfActive()
    {
        if (keepTween != null && keepTween.IsActive())
            keepTween.Kill();
        keepTween = null;
    }

    private Tween AdjustTween(Tween t)
    {
        return t.SetEase(ease)
                .SetAutoKill(!keepTheSameTween)
                .SetUpdate(isIndependantUpdate);
    }
}
