using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class waterSway : MonoBehaviour {


    public SpriteRenderer water;
    public float moveOffset = 0.5f;
    public float cycleDelay = 1f;

    private void Start()
    {
        Vector3 position = water.transform.position + new Vector3(moveOffset, 0f);

        /*Tweener Buoyancy =*/ water.transform.DOMove(position, cycleDelay)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
       
    }
}
