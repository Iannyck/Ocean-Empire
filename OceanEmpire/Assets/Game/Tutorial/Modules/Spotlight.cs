using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Tutorial
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Spotlight : MonoBehaviour
    {
        [Header("Links")]
        public CanvasGroup group;
        public Image centerFill;
        public Image centerHole;

        [Header("Fade in")]
        public float maxAlpha = 0.5f;
        public float fadeInDuration;
        public Ease fadeInEase = Ease.Linear;

        [Header("Fade out")]
        public float fadeOutDuration;
        public Ease fadeOutEase = Ease.Linear;

        [Header("Move")]
        public float moveDuration;
        public Ease moveEase = Ease.InOutSine;


        private Tweener fadeTween;
        private Tweener moveTween;
        private Tweener centerFillTween;
        private Tweener centerHoleTween;
        private bool isOn = false;

        private void Awake()
        {
            InstantOff();
        }

        public void InstantOff()
        {
            group.alpha = 0;
            isOn = false;
        }

        public void Off(TweenCallback onComplete = null)
        {
            if (fadeTween != null)
                fadeTween.Kill();

            fadeTween = group.DOFade(0, fadeOutDuration).SetEase(fadeOutEase).SetUpdate(true);

            isOn = false;

            if (onComplete != null)
                fadeTween.OnComplete(onComplete);
        }

        public void On(TweenCallback onComplete = null)
        {
            if (fadeTween != null)
                fadeTween.Kill();

            fadeTween = group.DOFade(maxAlpha, fadeInDuration).SetEase(fadeInEase).SetUpdate(true);

            isOn = true;

            if (onComplete != null)
                fadeTween.OnComplete(onComplete);
        }

        public void FillCenter(bool state, bool instant = false)
        {
            //Kill tweens
            if (centerFillTween != null && centerFillTween.IsActive())
                centerFillTween.Kill();
            if (centerHoleTween != null && centerHoleTween.IsActive())
                centerHoleTween.Kill();
            centerFillTween = null;
            centerHoleTween = null;

            //Calculate alphas
            float fillAlpha = state ? 1 : 0;
            float holeAlpha = 1 - fillAlpha;

            if (instant)
            {
                centerFill.SetAlpha(fillAlpha);
                centerHole.SetAlpha(holeAlpha);
            }
            else
            {
                centerFillTween = centerFill.DOFade(fillAlpha, 0.35f).SetUpdate(true);
                centerHoleTween = centerHole.DOFade(holeAlpha, 0.35f).SetUpdate(true);
            }
        }

        public void On(Vector2 absolutePosition, TweenCallback onComplete = null)
        {
            if (moveTween != null)
                moveTween.Kill();
            if (isOn)
                moveTween = GetComponent<RectTransform>().DOMove(absolutePosition, moveDuration).SetEase(moveEase).SetUpdate(true);
            else
                GetComponent<RectTransform>().position = absolutePosition;
            On(onComplete);
        }

        public void OnWorld(Vector2 worldPosition, TweenCallback onComplete = null)
        {
            Vector2 convertedPosition = Camera.main.WorldToScreenPoint(worldPosition);
            On(convertedPosition, onComplete);
        }
    }
}
