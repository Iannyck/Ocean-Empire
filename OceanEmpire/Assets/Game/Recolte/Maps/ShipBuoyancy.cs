using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipBuoyancy : MonoBehaviour
{

    public SpriteRenderer ship;
    public float moveOffset = 0.2f;
    public float cycleDelay = 1f;

    private void Start()
    {
        Vector3 top = ship.transform.position + new Vector3(0f, moveOffset);
        Vector3 Bottom = ship.transform.position - new Vector3(0f, moveOffset);

        Tweener first = ship.transform.DOMove(top, cycleDelay / 2);
        first.SetEase(Ease.OutSine);
        first.OnComplete(delegate ()
        {
            Tweener Buoyancy = ship.transform.DOMove(Bottom, cycleDelay)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        });
    }
}
