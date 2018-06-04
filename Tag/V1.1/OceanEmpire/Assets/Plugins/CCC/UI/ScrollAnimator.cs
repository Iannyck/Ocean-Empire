using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace CCC.UI
{
    /// <summary>
    /// Control animation and automation ScrollRect position
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollAnimator : MonoBehaviour, IBeginDragHandler
    {
        public bool isActive = false;

        [Tooltip("User will cancel active tweens when scrolling, if any")]
        public bool userCancelTween = true;

        [Header("Events"), System.NonSerialized]
        public UnityEvent onUserScroll = new UnityEvent();

        protected ScrollRect scrollRect;
        protected RectTransform container;

        protected RectTransform target;
        protected Vector2 targetContentAnchor;
        protected bool offsetFromContentSize;
        protected Tweener tween;

        protected Vector2 targetLocalPos;
        protected float totalVerticalSize;

        protected virtual void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            if (container == null) container = scrollRect.content;

            Vector3[] localCorners = new Vector3[4];
            scrollRect.GetComponent<RectTransform>().GetLocalCorners(localCorners);

            totalVerticalSize = Mathf.Abs(localCorners[0].y - localCorners[2].y);
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            // User drag will cancel tween
            if (userCancelTween) Cancel();
            onUserScroll.Invoke();
        }

        public virtual void Cancel()
        {
            ClearTween();
            isActive = false;
        }

        public virtual void ScrollTo(RectTransform target, float time = 0.6f, Ease ease = Ease.InOutQuint)
        {
            isActive = true;
            ClearTween();

            // Stop scroll velocity
            scrollRect.velocity = Vector2.zero;
            this.target = target;

            // Animate to new position
            targetLocalPos = GetRelativeLocalPos(target);
            tween = container.DOAnchorPosY(targetLocalPos.y, time).SetEase(ease).OnComplete(OnTweenComplete);
        }

        public void FastScrollTo(RectTransform target)
        {
            ClearTween();

            // Stop scroll velocity
            scrollRect.velocity = Vector2.zero;
            this.target = target;

            // Animate to new position
            targetLocalPos = GetRelativeLocalPos(target);
            Clamp();
        }

        protected virtual void ClearTween()
        {
            if (tween == null) return;
            tween.Kill();
            tween = null;
        }

        protected virtual void OnTweenComplete()
        {
            Clamp();
            tween = null;
            isActive = false;
        }

        protected virtual void Clamp()
        {
            // Clamp container to exact target position
            if (target != null)
            {
                Canvas.ForceUpdateCanvases();
                container.anchoredPosition = targetLocalPos;
            }
        }

        public Vector2 GetRelativeLocalPos(RectTransform target)
        {
            float yDif = (container.anchoredPosition.y - (scrollRect.transform.InverseTransformPoint(target.position)).y);

            float max = container.sizeDelta.y - totalVerticalSize;
            if (max < 0) max = 0;
            yDif = Mathf.Clamp(yDif, 0, max);

            return new Vector2(container.anchoredPosition.x, yDif);
        }
    }
}