using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        SubmarineMovement newPlayer = Spawn(new Vector2(0, 0));
        if (newPlayer == null)
            Debug.Log("Erreur Spawn Player");
        return newPlayer;
    }

    public SubmarineMovement SpawnFromTop()
    {
        SubmarineMovement newPlayer = Spawn(spawnPoint.transform.position);
        if (newPlayer == null)
            Debug.Log("Erreur Spawn Player");
        // Anim
        //newPlayer.transform.DOMove(new Vector3(0, Game.instance.map.heightMax - introAnimFromTopOffset), introAnimDuration);

        // Physics
        //newPlayer.SetTarget(new Vector2(0, Game.instance.map.heightMax - introAnimFromTopOffset));

        return newPlayer;
    }
}
