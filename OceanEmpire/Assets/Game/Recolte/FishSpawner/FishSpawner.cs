using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    public float delayBeforeSpawns = 2;
    private float lastSpawn;

    // Use this for initialization
    private SubmarineMovement submarine;
    private MapFishContent mapContent;
    private float maxDepth;
    private float depthScaling;



    void Start () {
        lastSpawn = delayBeforeSpawns;
    }
	
	// Update is called once per frame
	void Update () {
        if (Game.instance != null && (submarine == null))
        {
            MapInfo m = Game.instance.map;
            depthScaling = MapInfo.DEPTHSCALING;
            submarine = Game.instance.submarine;
            //mapContent = Game.instance.mapContent;
            return;
        }

        if (lastSpawn <= 0)
        {


        }



        //lastSpawn 

        //Time.deltaTime
	}
}
