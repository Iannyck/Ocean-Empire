using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenAnimation : MonoBehaviour {

    public Image bg;
    public Camera cam;

    public void Intro(UnityAction onComplete)
    {
        bg.DOFade(1, 1).OnComplete(delegate ()
        {
            Camera.main.gameObject.SetActive(false);
            cam.gameObject.SetActive(true);
            onComplete();
        }).SetUpdate(true);
    }

    public void Outro(UnityAction onComplete)
    {
        cam.gameObject.SetActive(false);
        bg.DOFade(0, 1).OnComplete(delegate ()
        {
            onComplete();
        }).SetUpdate(true);
    }

    public void OnNewSceneLoaded()
    {
        cam.gameObject.SetActive(false);
    }
}
