using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InGameTextPopup_Item : MonoBehaviour
{
    [SerializeField]
    private Text text;

    public float textFinalY;
    public float fadeInDuration;
    public float pause;
    public float fadeOutDuration;

    private Vector2 worldPosition;
    private Camera cam = null;
    private RectTransform tr;

    public void Setup(string message, Color color, Vector2 worldPosition)
    {
        tr = GetComponent<RectTransform>();
        tr.localScale = Vector3.one;
        text.text = message;
        text.color = color.ChangedAlpha(0);
        this.worldPosition = worldPosition;

        if (Game.instance != null)
            cam = Game.GameCamera.cam;

        Sequence sq = DOTween.Sequence();
        sq.Append(text.DOColor(color, fadeInDuration));
        sq.AppendInterval(pause);
        sq.Append(text.DOFade(0, fadeOutDuration));
        sq.OnComplete(this.DestroyGO);

        text.rectTransform.DOAnchorPosY(textFinalY, fadeInDuration + pause).SetEase(Ease.OutSine);
    }

    void Update()
    {
        if (cam != null)
        {
            tr.position = cam.WorldToScreenPoint(worldPosition);
        }
    }
}
