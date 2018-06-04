using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CCC.UI.Animation
{
    public class Popup : MonoBehaviour
    {

        /*
        Exemple d'une ligne de code qui fonctionnait et qui utilisait toutes les fonctionnalités
        Popup.CreatePopup(popup, transform).ChangeAnimationSettings(anchorMoveDelta).ChangeTextSettings(textSize, color, font).Animate(gameObject, text, useOutLine);
        */

        [Header("Components"), SerializeField]
        private Text textComponent;
        [SerializeField]
        private Image imageComponent;
        [SerializeField]
        private Outline outlineComponent;

        [Header("Animation Settings"), SerializeField]
        private Vector2 anchorMoveDelta = new Vector2(0, -200);
        [SerializeField]
        private float moveDuration = 0.5f;
        [SerializeField]
        private Ease moveEase = Ease.OutQuint;
        [SerializeField]
        private float delayBeforeFade = 0.5f;
        [SerializeField]
        private float fadeDuration = 0.5f;
        [SerializeField]
        private bool destroyOnComplete = true;

        public Text GetTextComponent() { return textComponent; }
        public Image GetImageComponent() { return imageComponent; }
        public Outline GetOutlineComponent() { return outlineComponent; }
        public RectTransform GetRectTransformComponent() { return transform as RectTransform; }

        /// <summary>
        /// Create and instance of the popup for you. Make it easier to call the anim in one line
        /// </summary>
        public static Popup CreatePopup(Popup popup, Transform transform)
        {
            Popup resultPopup = Instantiate(popup, transform).GetComponent<Popup>();
            return resultPopup;
        }

        /// <summary>
        /// Animate the text popup from origin
        /// </summary>
        public void Animate(GameObject origin, string text, bool useOutline)
        {
            InitialSettings(origin.transform.position, text, useOutline);

            InitAnime(true);
        }

        /// <summary>
        /// Animate the image popup from origin
        /// </summary>
        public void Animate(GameObject origin, Sprite sprite)
        {
            InitialSettings(origin.transform.position, sprite);

            InitAnime(false);
        }

        /// <summary>
        /// Change settings of the text in the popup
        /// </summary>
        public Popup ChangeTextSettings(int fontSize,
                                        Color color,
                                        Font font)
        {
            GetTextComponent().fontSize = fontSize;
            GetTextComponent().color = color;
            GetTextComponent().font = font;
            return this;
        }

        /// <summary>
        /// Change settings of the image in the popup
        /// </summary>
        public Popup ChangeImageSettings(Vector3 scale,
                                        Color color)
        {
            GetImageComponent().transform.localScale = scale;
            GetImageComponent().color = color;
            return this;
        }

        /// <summary>
        /// Change settings of the animation in the popup
        /// </summary>
        public Popup ChangeAnimationSettings(Vector2 anchorMoveDelta,
                                            float moveDuration = 0.5f,
                                            Ease moveEase = Ease.OutQuint,
                                            float delayBeforeFade = 0.5f,
                                            float fadeDuration = 0.5f)
        {
            this.anchorMoveDelta = anchorMoveDelta;
            this.moveDuration = moveDuration;
            this.moveEase = moveEase;
            this.delayBeforeFade = delayBeforeFade;
            this.fadeDuration = fadeDuration;
            return this;
        }

        private void InitAnime(bool isTextPopup = true)
        {
            RectTransform tr = GetRectTransformComponent();
            if (tr == null)
            {
                Debug.LogError("Popup needs to be put on a rect transform");
                return;
            }

            var moveAnim = tr.DOAnchorPos(anchorMoveDelta, moveDuration).SetEase(moveEase).SetRelative();
            Tweener fadeAnim;
            if (isTextPopup)
                fadeAnim = textComponent.DOFade(0, fadeDuration).SetDelay(delayBeforeFade);
            else
                fadeAnim = imageComponent.DOFade(0, fadeDuration).SetDelay(delayBeforeFade);

            if (destroyOnComplete)
            {
                if (fadeDuration + delayBeforeFade > moveDuration)
                    fadeAnim.onComplete = this.DestroyGO;
                else
                    moveAnim.onComplete = this.DestroyGO;
            }
        }

        private void InitialSettings(Vector3 pos, string text, bool outline)
        {
            GetTextComponent().enabled = true;
            GetImageComponent().enabled = false;
            GetOutlineComponent().enabled = outline;

            transform.position = pos;

            GetTextComponent().text = text;
        }

        private void InitialSettings(Vector3 pos, Sprite image)
        {
            GetTextComponent().enabled = false;
            GetOutlineComponent().enabled = false;
            GetImageComponent().enabled = true;

            transform.position = pos;

            GetImageComponent().sprite = image;
        }
    }
}
