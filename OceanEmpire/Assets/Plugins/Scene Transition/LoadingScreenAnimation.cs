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
    public new ParticleSystem particleSystem;
    public RawImage particleTexture;
    public float transitionDuration = 5f;
    public float waitTime = 5f;

    public void Intro(UnityAction onComplete)
    {
        //bg.DOFade(1, transitionDuration/2)
        particleTexture.gameObject.SetActive(true);
        particleSystem.gameObject.SetActive(true);
        particleTexture.DOFade(1, transitionDuration / 2).OnComplete(delegate ()
        {
            if (handleCameras)
            {
                Camera currentCam = Camera.main;
                if (currentCam != null)
                    currentCam.gameObject.SetActive(false);
                cam.gameObject.SetActive(true);
            }
            this.DelayedCall(() => { onComplete(); }, waitTime);
        }).SetUpdate(true);
    }

    public void Outro(UnityAction onComplete)
    {
        if (handleCameras)
            cam.gameObject.SetActive(false);
        //bg.DOFade(0, transitionDuration/2)
        particleTexture.DOFade(0, transitionDuration / 2).OnComplete(delegate ()
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
