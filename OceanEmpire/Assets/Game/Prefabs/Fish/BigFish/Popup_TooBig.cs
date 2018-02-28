using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_TooBig : MonoBehaviour
{
    private const float resetCooldown = 3;
    private float cooldown = 0;

    public void SpawnText(ColliderInfo info, Collision2D hit)
    {
        if (cooldown > 0)
            return;

        Game.Recolte_UI.textPopups.SpawnText("Trop gros!", new Color(1, 0.8f, 0.8f, 1), hit.contacts[0].point);
        cooldown = resetCooldown;
    }

    void Update()
    {
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
    }
}
