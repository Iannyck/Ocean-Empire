using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CoinsPopUp : MonoBehaviour
{

    public Image coinsImage;
    public Text moneyText;

    public float FadeOutTime;

    // Use this for initialization
    void Start()
    {
        Color imageColor = coinsImage.color;
        Color moneyColor = moneyText.color;

        imageColor.a = 0f;
        moneyColor.a = 0f;

        coinsImage.DOColor(imageColor, FadeOutTime);
        Tweener tw = moneyText.DOColor(imageColor, FadeOutTime);
        tw.onComplete = delegate { Destroy(gameObject); };
    }
}
