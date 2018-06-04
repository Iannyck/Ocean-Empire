using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireworksCamera : MonoBehaviour
{
    public ParticleSystem ps;
    public SpriteRenderer shine;
    public Camera cam;

    private int ongoingAnimations;

    public void Fire()
    {
        cam.enabled = true;
        ongoingAnimations++;
        shine.DOFade(1, 0.1f).onComplete = () => shine.DOFade(0, 0.5f).OnComplete(OnComplete);
        ps.Play();
    }

    void OnComplete()
    {
        ongoingAnimations--;
        if (ongoingAnimations <= 0)
            cam.enabled = false;
    }
}
