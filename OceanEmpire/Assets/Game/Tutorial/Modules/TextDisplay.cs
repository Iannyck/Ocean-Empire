using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Tutorial
{
    public class TextDisplay : MonoBehaviour
    {
        public CanvasGroup fade;
        public Text text;
        public Image blackFade;
        public Image characterImage;

        [Header("Fade settings")]
        public float fadeDuration;
        public Ease fadeEase;

        [Header("Height settings")]
        public Vector2 topPosition;
        public Vector2 middlePosition;
        public Vector2 bottomPosition;

        private bool isOn = false;

        void Awake()
        {
            InstantHide();
        }

        public void SetBottom()
        {
            GetComponent<RectTransform>().anchoredPosition = bottomPosition;
        }

        public void SetMiddle()
        {
            GetComponent<RectTransform>().anchoredPosition = middlePosition;
        }

        public void SetTop()
        {
            GetComponent<RectTransform>().anchoredPosition = topPosition;
        }

        public void InstantDisplay(string message, bool blackBackground)
        {
            fade.gameObject.SetActive(true);
            fade.alpha = 1;

            blackFade.enabled = blackBackground;

            text.text = message;

            isOn = true;
        }

        public void InstantHide()
        {
            fade.gameObject.SetActive(false);
            fade.alpha = 0;
            isOn = false;
        }

        public void DisplayText(string message, bool blackBackground, TweenCallback onComplete = null)
        {
            if (isOn)
            {
                HideText(delegate ()
                {
                    DisplayText(message, blackBackground, onComplete);
                });
            }
            else
            {
                fade.DOKill();
                fade.gameObject.SetActive(true);
                Tweener fadeTween = fade.DOFade(1, fadeDuration).SetEase(fadeEase).SetUpdate(true);

                text.text = message;

                blackFade.enabled = blackBackground;

                isOn = true;

                if (onComplete != null)
                    fadeTween.OnComplete(onComplete);
            }

        }

        public void HideText(TweenCallback onComplete = null)
        {
            fade.DOKill();
            fade.DOFade(0, fadeDuration).SetEase(fadeEase).SetUpdate(true).OnComplete(delegate ()
            {
                InstantHide();

                if (onComplete != null)
                    onComplete();
            });
        }

        public void SetCharacterSprite(Sprite character)
        {
            characterImage.enabled = true;
            characterImage.sprite = character;
        }
        public void DisableCharacterSprite()
        {
            characterImage.enabled = false;
        }
    }
}
