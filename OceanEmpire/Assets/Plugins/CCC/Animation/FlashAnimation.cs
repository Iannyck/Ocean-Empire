using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashAnimation : MonoBehaviour
{

    public float fadeDuration = 1f;
    public bool timeScaleIndependant = false;
    public bool transparentAtInit = false;
    public bool initOnStart = true;

    public enum ComponentType { noFade = 0, text = 1, image = 2, sprite = 3 }
    public ComponentType currentType;

    private bool stopAnim;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (currentType == ComponentType.noFade)
            return;

        stopAnim = false;

        switch (currentType)
        {
            case ComponentType.text:
                if (GetComponent<Text>() == null)
                    return;
                if (transparentAtInit)
                    GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 1);
                else
                    GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, 0);
                break;
            case ComponentType.image:
                if (GetComponent<Image>() == null)
                    return;
                if (transparentAtInit)
                    GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 1);
                else
                    GetComponent<Image>().color = new Color(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b, 0);
                break;
            case ComponentType.sprite:
                if (GetComponent<SpriteRenderer>() == null)
                    return;
                if (transparentAtInit)
                    GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1);
                else
                    GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0);
                break;
            default:
                break;
        }

        if (transparentAtInit)
            FadeIn();
        else
            FadeOut();
    }

    void FadeIn()
    {
        Tweener myAnimation = null;

        switch (currentType)
        {
            case ComponentType.text:
                myAnimation = GetComponent<Text>().DOFade(1, fadeDuration).SetUpdate(timeScaleIndependant);
                break;
            case ComponentType.image:
                myAnimation = GetComponent<Image>().DOFade(1, fadeDuration).SetUpdate(timeScaleIndependant);
                break;
            case ComponentType.sprite:
                myAnimation = GetComponent<SpriteRenderer>().DOFade(1, fadeDuration).SetUpdate(timeScaleIndependant);
                break;
            default:
                break;
        }

        if (myAnimation == null)
            return;

        if (stopAnim)
            return;

        myAnimation.OnComplete(FadeOut);
    }

    void FadeOut()
    {
        Tweener myAnimation = null;

        if (timeScaleIndependant)
            myAnimation.SetUpdate(true);

        switch (currentType)
        {
            case ComponentType.text:
                myAnimation = GetComponent<Text>().DOFade(0, fadeDuration).SetUpdate(timeScaleIndependant);
                break;
            case ComponentType.image:
                myAnimation = GetComponent<Image>().DOFade(0, fadeDuration).SetUpdate(timeScaleIndependant);
                break;
            case ComponentType.sprite:
                myAnimation = GetComponent<SpriteRenderer>().DOFade(0, fadeDuration).SetUpdate(timeScaleIndependant);
                break;
            default:
                break;
        }

        if (myAnimation == null)
            return;

        if (stopAnim)
            return;

        myAnimation.OnComplete(FadeIn);
    }

    public void StopFlashAnim()
    {
        stopAnim = true;
    }
}
