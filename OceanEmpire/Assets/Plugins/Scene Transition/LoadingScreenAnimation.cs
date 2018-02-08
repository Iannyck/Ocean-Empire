using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreenAnimation : MonoBehaviour
{
    public Image bg;
    public bool handleCameras = true;
    public Camera cam;

    public void Intro(UnityAction onComplete)
    {
        bg.DOFade(1, 1).OnComplete(delegate ()
        {
            if (handleCameras)
            {
                Camera currentCam = Camera.main;
                if (currentCam != null)
                    currentCam.gameObject.SetActive(false);
                cam.gameObject.SetActive(true);
            }
            onComplete();
        }).SetUpdate(true);
    }

    public void Outro(UnityAction onComplete)
    {
        if (handleCameras)
            cam.gameObject.SetActive(false);
        bg.DOFade(0, 1).OnComplete(delegate ()
        {
            onComplete();
        }).SetUpdate(true);
    }

    public void OnNewSceneLoaded()
    {
        if (handleCameras)
            cam.gameObject.SetActive(false);
    }
}
