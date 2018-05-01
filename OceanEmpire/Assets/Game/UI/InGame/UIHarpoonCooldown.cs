using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHarpoonCooldown : MonoBehaviour
{
    public Image ring;

    SlingshotControl slingshot;
    Transform playerTr;
    Transform tr;

    void Start()
    {
        tr = transform;
        Game.OnGameReady += Game_OnGameReady;
    }

    private void Game_OnGameReady()
    {
        slingshot = Game.Instance.Submarine.GetComponent<SlingshotControl>();
        playerTr = slingshot.transform;
    }

    void Update()
    {
        if (slingshot != null)
        {
            if (slingshot.IsInCooldown())
            {
                if (!ring.enabled)
                    ring.enabled = true;
                ring.fillAmount = slingshot.RemainingCooldown / slingshot.HarpoonCooldown;
            }
            else
            {
                if (ring.enabled)
                    ring.enabled = false;
            }
        }

        if(playerTr != null)
        {
            var pixelPos = Game.Instance.GameCamera.CameraComponent.WorldToScreenPoint(playerTr.position);
            tr.position = pixelPos;
        }
    }
}
