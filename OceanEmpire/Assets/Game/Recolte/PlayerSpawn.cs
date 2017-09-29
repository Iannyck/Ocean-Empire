using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerSpawn : MonoBehaviour
{

    public SubmarineMovement submarinePrefab;
    public GameObject spawnPoint;

    public float introAnimDuration = 1f;
    public float introAnimFromTopOffset = 1;

    public SubmarineMovement Spawn(Vector2 position)
    {
        if (submarinePrefab == null)
            return null;

        return Game.Spawner.Spawn(submarinePrefab, position);
    }

    public SubmarineMovement SpawnPlayer()
    {
        SubmarineMovement newPlayer = Spawn(spawnPoint.transform.position);
        if (newPlayer == null)
            Debug.Log("Erreur Spawn Player");
        return newPlayer;
    }

    public SubmarineMovement AnimatePlayerFromTop(SubmarineMovement player, Action onIntroAnimComplete)
    {
        // Physics
        player.enabled = false;

        // Anim
        player.transform.DOMove(new Vector3(0,
            Game.instance.map.mapTop - introAnimFromTopOffset),
            introAnimDuration).OnComplete(delegate ()
            {
                player.enabled = true;
                onIntroAnimComplete();
            });



        return player;
    }
}
