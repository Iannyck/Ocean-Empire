using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerSpawn : MonoBehaviour {

    public SubmarineMovement submarinePrefab;
    public GameObject spawnPoint;

    public float introAnimDuration = 1f;
    public float introAnimFromTopOffset = 1;

    public SubmarineMovement Spawn(Vector2 position)
    {
        if (submarinePrefab == null)
            return null;

        return Instantiate(submarinePrefab.gameObject, (Vector3)position, Quaternion.identity).GetComponent<SubmarineMovement>();
    }

    public SubmarineMovement SpawnPlayer()
    {
        SubmarineMovement newPlayer = Spawn(spawnPoint.transform.position);
        if (newPlayer == null)
            Debug.Log("Erreur Spawn Player");
        return newPlayer;
    }

    public SubmarineMovement SpawnFromTop(Action onIntroAnimComplete)
    {
        SubmarineMovement newPlayer = Spawn(spawnPoint.transform.position);
        if (newPlayer == null)
            Debug.Log("Erreur Spawn Player");

        // Physics
        newPlayer.enabled = false;

        // Anim
        newPlayer.transform.DOMove(new Vector3(0, 
            Game.instance.map.heightMax - introAnimFromTopOffset), 
            introAnimDuration).OnComplete(delegate() {
                onIntroAnimComplete();
                newPlayer.enabled = true;
            });



        return newPlayer;
    }
}
