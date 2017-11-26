using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CCC.Manager;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MessagePopup : MonoBehaviour
{
    [SerializeField, Header("Links")]
    private Text text;
    [SerializeField]
    private Image bgImage;

    [SerializeField, Header("Settings")]
    private float openDuration = 0.35f;
    [SerializeField]
    private Ease openEase = Ease.OutQuad;
    [SerializeField]
    private float startHorizontal = 0.5f;

    [SerializeField]
    private float stayDuration = 1.5f;

    [SerializeField]
    private float hideDuration;
    [SerializeField]
    private Ease hideEase = Ease.InSine;

    private Vector2 destinedSizeDelta;

    private const string SCENENAME = "MessagePopup";

    public static void DisplayMessage(string message, TweenCallback onComplete = null)
    {
        Scenes.LoadAsync(SCENENAME, LoadSceneMode.Additive, (scene) =>
        {
            scene.FindRootObject<MessagePopup>().DisplayText(message, onComplete);
        }, false);
    }

    private void Awake()
    {
        RectTransform imageRT = bgImage.rectTransform;
        destinedSizeDelta = imageRT.sizeDelta;

        imageRT.sizeDelta -= new Vector2(((1 - startHorizontal) * imageRT.rect.size.x), imageRT.sizeDelta.y);

        text.color = text.color.ChangedAlpha(0);
    }

    private void DisplayText(string message, TweenCallback onComplete = null)
    {
        text.text = message;

        Sequence sq = DOTween.Sequence();

        //Bg appear
        sq.Append(bgImage.rectTransform.DOSizeDelta(destinedSizeDelta, openDuration).SetEase(openEase));

        //Text fade in
        sq.Insert(openDuration * 0.6f, text.DOFade(1, openDuration * 0.4f));


        //Pause
        sq.AppendInterval(stayDuration);


        //Bg disappear;
        sq.Append(bgImage.DOFade(0, hideDuration).SetEase(hideEase));

        //Text disappear;
        sq.Join(text.DOFade(0, hideDuration).SetEase(hideEase));


        sq.OnComplete(() =>
        {
            if (onComplete != null)
                onComplete();

            Scenes.UnloadAsync(gameObject.scene);
        });

        
    }
}
