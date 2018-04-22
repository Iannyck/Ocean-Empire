using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Questing;
using DG.Tweening;

public class QuestPanelEntry : MonoBehaviour
{
    [Header("Components"), SerializeField] Text description;
    [SerializeField] Text status;
    [SerializeField] Image icon;
    [SerializeField] Slider slider;
    [SerializeField] Image sliderFill;
    [SerializeField] Image background;
    [SerializeField] Image whiteOverlay;

    [Header("Sprites"), SerializeField] Sprite ongoingBackground;
    [SerializeField] Sprite completedBackground;
    [SerializeField] Sprite ongoingSliderFill;
    [SerializeField] Sprite completedSliderFill;
    [SerializeField] float completedIconAlpha;

    [Header("Flash Away Animation")]
    [SerializeField] float fa_fadeInDuration = 0.25f;
    [SerializeField] Ease fa_fadeInEase = Ease.OutSine;
    [SerializeField] float fa_pause = 0;
    [SerializeField] float fa_fadeOutDuration = 0.25f;
    [SerializeField] Ease fa_fadeOutEase = Ease.InSine;

    public void Fill(Quest quest)
    {
        description.text = quest.Context.description;
        if (quest.state == QuestState.Completed)
        {
            status.text = "COMPLÉTÉ !";
            sliderFill.sprite = completedSliderFill;
            background.sprite = completedBackground;
            icon.color = new Color(1, 1, 1, completedIconAlpha);
        }
        else
        {
            status.text = "En cours: " + quest.GetDisplayedProgressText();
            sliderFill.sprite = ongoingSliderFill;
            background.sprite = ongoingBackground;
            icon.color = new Color(1, 1, 1, 1);
        }

        Sprite iconSprite = Resources.Load<Sprite>(quest.Context.iconResource);

        icon.sprite = iconSprite;
        icon.enabled = iconSprite != null;

        slider.value = quest.GetProgress01();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        FlashAwayAnimation();
    //    }
    //}

    public void FlashAwayAnimation(TweenCallback onComplete = null)
    {
        whiteOverlay.gameObject.SetActive(true);
        whiteOverlay.SetAlpha(0);

        var sq = DOTween.Sequence();
        sq.Join(whiteOverlay.DOFade(1, fa_fadeInDuration).SetEase(fa_fadeInEase));
        sq.AppendCallback(() =>
        {
            if (this == null || whiteOverlay == null)
                return;
            var overlayTR = whiteOverlay.transform;
            foreach (Transform child in transform)
            {
                if (child != overlayTR)
                    child.gameObject.SetActive(false);
            }
        });
        sq.AppendInterval(fa_pause);
        sq.Append(whiteOverlay.DOFade(0, fa_fadeOutDuration).SetEase(fa_fadeOutEase));
        sq.onComplete = onComplete;
    }
}
