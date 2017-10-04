using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFlash : MonoBehaviour
{

    public float fadeDuration = 1f;
    public bool timeScaleIndependant = false;
    public bool transparentAtStart = false;
    public bool flashOnStart = true;
    public Ease ease = Ease.Linear;

    public enum ComponentType { noFade = 0, Text = 1, Image = 2, Sprite = 3, CanvasGroup = 4 }
    public ComponentType currentType = ComponentType.noFade;

    private bool stopAnim;
    private Tween tween;

    void Start()
    {
        if (flashOnStart)
            Flash();
    }
    public void Stop()
    {
        if (tween != null && tween.IsActive())
            tween.Kill();
    }

    public void Play()
    {
        if (tween != null && tween.IsActive())
            tween.Play();
        else
            Flash();
    }

    public void Pause()
    {
        if (tween != null && tween.IsPlaying())
            tween.Pause();
    }

    void Flash()
    {
        if (currentType == ComponentType.noFade)
            return;

        Stop();


        switch (currentType)
        {
            case ComponentType.Text:
                {
                    Text text = GetComponent<Text>();
                    if (text == null)
                        return;

                    Color c = text.color;
                    float defaultAlpha = c.a;

                    if (transparentAtStart)
                        text.color = c.ChangedAlpha(0);

                    tween = text.DOFade(transparentAtStart ? defaultAlpha : 0, fadeDuration);

                    break;
                }
            case ComponentType.Image:
                {
                    Image image = GetComponent<Image>();
                    if (image == null)
                        return;

                    Color c = image.color;
                    float defaultAlpha = c.a;

                    if (transparentAtStart)
                        image.color = c.ChangedAlpha(0);

                    tween = image.DOFade(transparentAtStart ? defaultAlpha : 0, fadeDuration);

                    break;
                }
            case ComponentType.Sprite:
                {
                    SpriteRenderer sprRenderer = GetComponent<SpriteRenderer>();
                    if (sprRenderer == null)
                        return;

                    Color c = sprRenderer.color;
                    float defaultAlpha = c.a;

                    if (transparentAtStart)
                        sprRenderer.color = c.ChangedAlpha(0);

                    tween = sprRenderer.DOFade(transparentAtStart ? defaultAlpha : 0, fadeDuration);

                    break;
                }
            case ComponentType.CanvasGroup:
                {
                    CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
                    if (canvasGroup == null)
                        return;

                    float defaultAlpha = canvasGroup.alpha;

                    if (transparentAtStart)
                        canvasGroup.alpha = 0;

                    tween = canvasGroup.DOFade(transparentAtStart ? defaultAlpha : 0, fadeDuration);

                    break;
                }
            default:
                break;
        }

        if (tween != null)
            tween.SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
    }

}
