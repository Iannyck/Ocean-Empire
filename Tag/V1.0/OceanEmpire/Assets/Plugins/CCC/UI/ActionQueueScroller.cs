using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using CCC.Utility;

namespace CCC.UI
{
    public class ActionQueueScroller : MonoBehaviour
    {
        public ScrollRect scroll;
        public ActionQueue listenQueue;
        public ScrollAnimator scrollAnimator;

        void Awake()
        {
            if (listenQueue == null)
            {
                Debug.LogWarning("The ActionQueue reference is null and it shouldn't.");
                return;
            }
            listenQueue.onNextItem.AddListener(OnNextItem);
        }

        public void FastScrollTo(RectTransform target)
        {
            if (target == null) return;
             
            scrollAnimator.FastScrollTo(target);
        }

        public void ScrollTo(RectTransform target)
        {
            if (target == null) return;
            scrollAnimator.ScrollTo(target);
        }

        void OnNextItem(ActionQueue.Action action)
        {
            if (action.target == null) return;

            RectTransform rect = action.target.GetComponent<RectTransform>();

            ScrollTo(rect);
        }
    }
}
