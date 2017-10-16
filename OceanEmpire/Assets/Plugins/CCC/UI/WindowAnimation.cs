using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using CCC.Manager;

namespace CCC.UI
{
    public class WindowAnimation : MonoBehaviour
    {

        [Header("Components")]
        public RectTransform windowBg;
        public CanvasGroup content;
        public Image backBg;

        [Header("Window Bg Settings")]
        [Range(0, 1)]
        public float verticalStart = 1;
        [Range(0, 1)]
        public float horizontalStart = 0;
        [Range(0, 1)]
        public float fadeStart = 1;
        public bool openOnAwake = true;

        [Header("Open")]
        public float openTime = 0.35f;
        public Ease openEase = Ease.OutSine;

        [Header("Exit")]
        public float exitTime = 0.35f;
        public Ease exitEase = Ease.InSine;
        public bool instantHideContent = false;

        [Header("Size")]
        public bool autoDetectSize = true;
        public Vector2 size;


        Vector2 smallV;
        Vector2 bigV;

        private RectTransform bgTr;
        private Image bgImage;
        private float bgImageAlpha;
        private bool isOpen = false;
        private float backBgAlpha;

        protected virtual void Awake()
        {
            bgTr = windowBg.GetComponent<RectTransform>();
            bgImage = windowBg.GetComponent<Image>();
            if (bgImage)
                bgImageAlpha = bgImage.color.a;
            if (backBg)
                backBgAlpha = backBg.color.a;

            bigV = autoDetectSize ? bgTr.sizeDelta : size;
            smallV = new Vector2(bigV.x * horizontalStart, bigV.y * verticalStart);

            InstantClose();

            if (openOnAwake)
                Open();
        }

        public void Open(TweenCallback onComplete = null)
        {
            isOpen = true;
            bgTr.gameObject.SetActive(true);

            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.DOFade(bgImageAlpha, openTime * 0.75f).SetUpdate(true);
            }

            if (backBg != null)
            {
                backBg.gameObject.SetActive(true);
                backBg.DOKill();
                backBg.DOFade(backBgAlpha, openTime).SetUpdate(true);
            }

            if (content != null)
            {
                content.gameObject.SetActive(true);


                bgTr.DOKill();
                bgTr.DOSizeDelta(bigV, openTime).SetEase(openEase).SetUpdate(true);

                content.DOKill();
                content.DOFade(1, openTime).SetDelay(openTime * 0.75f).SetEase(openEase).OnComplete(delegate ()
                {
                    content.blocksRaycasts = true;
                    if (onComplete != null)
                        onComplete.Invoke();
                }).SetUpdate(true);
            }
            else
            {
                bgTr.DOKill();
                bgTr.DOSizeDelta(bigV, openTime).SetEase(openEase).OnComplete(onComplete).SetUpdate(true);
            }
        }

        public void Close(TweenCallback onComplete = null)
        {
            isOpen = false;

            float delay = content == null || instantHideContent ? 0 : exitTime * 0.75f;


            //Le content se fade-out en premier
            if (content != null)
            {
                content.DOKill();
                if (instantHideContent)
                    content.alpha = 0;
                else
                    content.DOFade(0, exitTime * 0.75f).SetEase(exitEase).SetUpdate(true);
                content.blocksRaycasts = false;
            }

            //Le reste a du délai (potentiellement)
            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.DOFade(fadeStart, exitTime).SetDelay(delay).SetUpdate(true);
            }

            if (backBg != null)
            {
                backBg.DOKill();
                backBg.DOFade(0, exitTime + delay).SetUpdate(true);//.SetDelay(delay);
            }

            bgTr.DOKill();
            bgTr.DOSizeDelta(smallV, exitTime).SetDelay(delay).SetEase(exitEase).OnComplete(delegate ()
            {
                bgTr.gameObject.SetActive(false);
                if (onComplete != null)
                    onComplete.Invoke();
            }).SetUpdate(true);
        }

        public void InstantOpen()
        {
            bgTr.DOKill();
            bgTr.sizeDelta = bigV;
            bgTr.gameObject.SetActive(true);

            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.SetAlpha(bgImageAlpha);
            }

            if (backBg != null)
            {
                backBg.DOKill();
                backBg.SetAlpha(backBgAlpha);
                backBg.gameObject.SetActive(true);
            }

            if (content != null)
            {
                content.DOKill();
                content.blocksRaycasts = true;
                content.alpha = 1;
                content.gameObject.SetActive(true);
            }
            isOpen = true;
        }

        public void InstantClose()
        {
            isOpen = false;
            bgTr.DOKill();
            bgTr.sizeDelta = smallV;
            bgTr.gameObject.SetActive(false);

            if (bgImage != null)
            {
                bgImage.DOKill();
                bgImage.SetAlpha(fadeStart);
            }

            if (backBg != null)
            {
                backBg.DOKill();
                backBg.SetAlpha(0);
                backBg.gameObject.SetActive(false);
            }

            if (content != null)
            {
                content.DOKill();
                content.blocksRaycasts = false;
                content.alpha = 0;
                content.gameObject.SetActive(false);
            }
        }

        public virtual void QuitScene()
        {
            if (isOpen)
                Close(UnloadScene);
            else
                UnloadScene();
        }

        private void UnloadScene()
        {
            Scenes.UnloadAsync(gameObject.scene.name);
        }

        public bool IsOpen() { return isOpen; }
    }
}
