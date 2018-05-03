using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop2UI : MonoBehaviour
{
    public RectTransform panel;
    public Vector2 hideDeltaPos;
    public float arriveDuration;
    public AnimationCurve arriveEase;
    public float hideDuration;
    public Ease hideEase;
    public Image blackBG;

    Vector2 normalPosition;
    float normalBlackAlpha;

    private bool isQuitting = false;

    void Awake()
    {
        normalPosition = panel.anchoredPosition;
        normalBlackAlpha = blackBG.color.a;
    }

    void OnEnable()
    {
        HideInstant();
        Show(null);
    }

    void Show(TweenCallback onComplete)
    {
        this.DOKill();

        blackBG.DOFade(normalBlackAlpha, arriveDuration);

        panel.DOAnchorPos(normalPosition, arriveDuration).SetEase(arriveEase).onComplete = onComplete;
    }

    void HideInstant()
    {
        blackBG.SetAlpha(0);
        blackBG.raycastTarget = false;
        panel.anchoredPosition = normalPosition + hideDeltaPos;
    }

    void Hide(TweenCallback onComplete)
    {
        this.DOKill();

        blackBG.DOFade(0, arriveDuration);
        blackBG.raycastTarget = false;

        panel.DOAnchorPos(normalPosition + hideDeltaPos, hideDuration).SetEase(hideEase).onComplete = onComplete;
    }
    public void QuitShop()
    {
        if (isQuitting)
            return;

        isQuitting = true;
        Hide(() =>
        {
            isQuitting = false;
            Scenes.UnloadAsync(gameObject.scene);

            if (Scenes.IsActive("Shack"))
            {
                Scenes.GetActive("Shack").FindRootObject<Shack>().ForceUpdateFishingFrenzyDisplay();
            }
        });
    }
}
