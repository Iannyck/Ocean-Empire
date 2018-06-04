using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPopup : MonoBehaviour
{
    [Header("Components"), SerializeField] private Text textComponent;
    [SerializeField] private Outline outlineComponent;

    [Header("Animation Settings"), SerializeField] private bool animateOnStart = true;
    [SerializeField] private Vector2 anchorMoveDelta = new Vector2(0, -200);
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Ease moveEase = Ease.OutQuint;
    [SerializeField] private float delayBeforeFade = 0.5f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool destroyOnComplete = true;

    public Text GetTextComponent() { return textComponent; }
    public Outline GetOutlineComponent() { return outlineComponent; }
    public RectTransform GetRectTransformComponent() { return transform as RectTransform; }

    private void Start()
    {
        if (animateOnStart)
            Animate();
    }

    public void Animate()
    {
        RectTransform tr = GetRectTransformComponent();
        if(tr == null)
        {
            Debug.LogError("Text Popup needs to be put on a rect transform");
            return;
        }

        var moveAnim = tr.DOAnchorPos(anchorMoveDelta, moveDuration).SetEase(moveEase).SetRelative();
        var fadeAnim = textComponent.DOFade(0, fadeDuration).SetDelay(delayBeforeFade);

        if (destroyOnComplete)
        {
            if (fadeDuration + delayBeforeFade > moveDuration)
                fadeAnim.onComplete = this.DestroyGO;
            else
                moveAnim.onComplete = this.DestroyGO;
        }
    }
}
