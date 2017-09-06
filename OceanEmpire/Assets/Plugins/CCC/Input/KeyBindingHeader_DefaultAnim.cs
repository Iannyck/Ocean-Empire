using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace CCC.Input
{
    public class KeyBindingHeader_DefaultAnim : CCC.Animation.BaseAnimator
    {
        public RectTransform resetButton;

        [System.NonSerialized]
        public Color white;
        [System.NonSerialized]
        public Color black;
        [System.NonSerialized]
        bool colorSet = false;

        public override void Animate(GameObject target, params object[] extra)
        {
            if (!colorSet)
            {
                white = target.GetComponent<KeyBindingHeader>().button.image.color;
                black = target.GetComponent<KeyBindingHeader>().text.color;
                colorSet = true;
            }

            bool state = (bool)extra[0];

            target.GetComponent<KeyBindingHeader>().button.image.color = state ? black : white;
            target.GetComponent<KeyBindingHeader>().text.color = state ? white : black;

            resetButton.DOKill();
            resetButton.DOAnchorPosY(state ? 50 : 25, 0.25f).SetEase(Ease.OutSine);
        }
    }
}
