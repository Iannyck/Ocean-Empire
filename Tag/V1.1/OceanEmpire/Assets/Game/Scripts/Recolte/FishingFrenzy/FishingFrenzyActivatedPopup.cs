using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingFrenzyActivatedPopup : MonoBehaviour
{
    [Header("References"), SerializeField] RectTransform shine;
    [SerializeField] RectTransform container;
    [SerializeField] CanvasGroup canvasGroup;

    [Header("Shine Spin"), SerializeField] float spinLoops = 4;

    [Header("Durations"), SerializeField] float midDuration;
    [SerializeField] float pauseDuration;
    [SerializeField] float endDuration;

    [Header("Screen Positions [0,1]"), SerializeField]
    Vector2 startPosition;
    [SerializeField] Ease midPositionEase;
    [SerializeField] Vector2 midPosition;
    [SerializeField] Ease endPositionEase;
    [SerializeField] Vector2 endPosition;

    [Header("Sizes"), SerializeField] float startSize;
    [SerializeField] float midSizeExtraDelay;
    [SerializeField] Ease midSizeEase;
    [SerializeField] float midSize;
    [SerializeField] Ease endSizeEase;
    [SerializeField] float endSize;

    [Header("Other Settings"), SerializeField] bool destroyOnComplete;

    public void Animate()
    {
        Sequence sq = DOTween.Sequence();

        // Shine rotation
        shine.DORotate(Vector3.forward * 360 * spinLoops,
            midDuration + midSizeExtraDelay + pauseDuration + endDuration,
            RotateMode.LocalAxisAdd);

        // Start
        container.localScale = Vector3.one * startSize;
        container.position = ScreenPosConversion(startPosition);
        canvasGroup.alpha = 1;

        // Mid
        sq.Append(container.DOMove(ScreenPosConversion(midPosition), midDuration).SetEase(midPositionEase));
        sq.Join(container.DOScale(midSize, midDuration).SetEase(midSizeEase).SetDelay(midSizeExtraDelay));

        // Pause
        sq.AppendInterval(pauseDuration);

        // End
        sq.Append(container.DOMove(ScreenPosConversion(endPosition), endDuration).SetEase(endPositionEase));
        sq.Join(container.DOScale(endSize, endDuration).SetEase(endSizeEase));
        sq.Join(canvasGroup.DOFade(0, endDuration));

        sq.onComplete = () =>
        {
            if (destroyOnComplete && container != null)
                Destroy(container.gameObject);
        };
    }

    private Vector3 ScreenPosConversion(Vector2 screenPos)
    {
        return new Vector3(Screen.width * screenPos.x, Screen.height * screenPos.y, 0);
    }
}
