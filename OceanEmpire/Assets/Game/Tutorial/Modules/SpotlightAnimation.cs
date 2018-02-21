using CCC.UI;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class SpotlightAnimation : MonoBehaviour
    {

        public float fadeDuration = 0.75f;
        public float baseAlpha = 0.5f;
        public CanvasGroup canvasGroup;

        public void Init(GameObject canvas)
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(baseAlpha, fadeDuration).SetUpdate(true);
        }

        public void Close(Action onComplete)
        {
            canvasGroup.alpha = baseAlpha;
            canvasGroup.DOFade(0, fadeDuration).SetUpdate(true).OnComplete(delegate () {
                onComplete.Invoke();
            });
            gameObject.SetActive(false);
        }
    }
}
