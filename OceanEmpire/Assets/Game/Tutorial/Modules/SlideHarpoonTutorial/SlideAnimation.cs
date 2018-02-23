using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideAnimation : MonoBehaviour {

    public GameObject hand;
    public float displacement = 125;
    public float duration = 1;

	public void Init(Vector3 origin)
    {
        transform.localPosition = origin;
        StartAnimation();
    }

    private void StartAnimation()
    {
        hand.transform.DOMoveX(transform.position.x - displacement, duration).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
    }

    private void Update()
    {
        transform.position = Game.GameCamera.cam.WorldToScreenPoint(Game.Instance.submarine.transform.position);
    }
}
