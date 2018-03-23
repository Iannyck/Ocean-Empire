using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace CCC.UI.Animation
{
    public class FloatingAnimation : MonoBehaviour
    {
        [Header("Settings")]
        public bool animateOnEnable = true;
        public bool stopOnDisable = true;
        public bool independantUpdate = true;

        [Header("Animation")]
        public float cycleDuration = 1;
        public float maxSize = 1.15f;
        public float minSize = 0.85f;
        public Ease ease = Ease.InOutSine;

        private Tween tween;
        private Transform tr;
        private float normalScale = 1;

        void Awake()
        {
            tr = transform;
            normalScale = tr.localScale.x;
        }

        void OnEnable()
        {
            if (animateOnEnable)
                Animate();
        }

        void OnDisable()
        {
            if (stopOnDisable)
                Stop();
        }

        public void Animate()
        {
            KillTween();

            tr.localScale = Vector3.one * minSize;

            float duration = Mathf.Max(0.02f, cycleDuration / 2);
            tween = tr.DOScale(maxSize, duration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(independantUpdate);
            tween.Goto(duration / 2, true);
        }

        public void Stop()
        {
            KillTween();
            tr.localScale = Vector3.one * normalScale;
        }

        void KillTween()
        {
            if (tween != null && tween.IsActive())
                tween.Kill();
            tween = null;
        }
    }
}

