using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaitAnimation : MonoBehaviour {

    public float animDuration = 2;
	
	public void DoAnimation ()
    {
        gameObject.transform.DORotate(new Vector3(0, 0, -180), animDuration).SetLoops(-1,LoopType.Incremental);
	}
}
