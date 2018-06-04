using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyFireworks : MonoBehaviour
{
    public FireworksCamera fireworks;
    public RawImage[] images;

    public void LaunchAnim()
    {
        StartCoroutine(AnimRoutine());
    }

    IEnumerator AnimRoutine()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = true;
        }
        Fire();
        yield return new WaitForSecondsRealtime(0.65f);
        Fire();
        yield return new WaitForSecondsRealtime(0.65f);

        for (int i = 0; i < images.Length; i++)
        {
            images[i].enabled = false;
        }
    }

    private void Fire()
    {
        if (fireworks != null)
            fireworks.Fire();
    }
}
