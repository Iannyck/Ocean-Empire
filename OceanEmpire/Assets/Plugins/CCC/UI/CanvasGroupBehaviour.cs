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
    private bool keepTheSameTween = false;

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

    public virtual void Hide()
    {
        IsShown = false;

        if (keepTheSameTween)
        {
            CheckLaunchTween(0);
            keepTween.PlayForward();
        }
        else
        {
            KillIfActive();
            CheckLaunchTween(0);
        }

        if (setBlocksRaycast)
            canvasGroup.blocksRaycasts = false;
        if (setInteractable)
            canvasGroup.interactable = false;
    }

    public virtual void Show()
    {
        IsShown = true;

        if (keepTheSameTween)
        {
            CheckLaunchTween(0);
            keepTween.PlayBackwards();
        }
        else
        {
            KillIfActive();
            CheckLaunchTween(1);
        }

        if (setBlocksRaycast)
            canvasGroup.blocksRaycasts = true;
        if (setInteractable)
            canvasGroup.interactable = true;
    }

    public virtual void HideInstant()
    {
        IsShown = false;

        if (keepTheSameTween)
        {
            if (keepTween != null && keepTween.IsActive())
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

        if (setBlocksRaycast)
            canvasGroup.blocksRaycasts = false;
        if (setInteractable)
            canvasGroup.interactable = false;
    }

    public virtual void ShowInstant()
    {
        IsShown = true;

        if (keepTheSameTween)
        {
            if (keepTween != null && keepTween.IsActive())
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
