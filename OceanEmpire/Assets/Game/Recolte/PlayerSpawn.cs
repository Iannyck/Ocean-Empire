using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSpawn : MonoBehaviour {

    public SubmarineMovement submarine;
    public GameObject spawnPoint;

    public float introAnimDuration = 1f;
    public float introAnimFromTopOffset = 1;

    public SubmarineMovement Spawn(Vector2 position)
    {
        if (submarine == null)
            return null;

        return Instantiate(submarine.gameObject, (Vector3)position, Quaternion.identity).GetComponent<SubmarineMovement>();
    }

    public void SpawnPlayer()
    {
        if (Spawn(new Vector2(0, 0)) == null)
            Debug.Log("Erreur Spawn Player");
    }

    public void SpawnFromTop()
    {
        SubmarineMovement newPlayer = Spawn(spawnPoint.transform.position);
        if (newPlayer == null)
            Debug.Log("Erreur Spawn Player");
        // Anim
        //newPlayer.transform.DOMove(new Vector3(0, Game.instance.map.heightMax - introAnimFromTopOffset), introAnimDuration);

        // Physics
        //newPlayer.SetTarget(new Vector2(0, Game.instance.map.heightMax - introAnimFromTopOffset));
    }
}
