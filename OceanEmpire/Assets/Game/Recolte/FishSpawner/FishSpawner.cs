using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    public FishPool fishPool;

    public float delayBeforeSpawns = 2;
    public float spawnAreaHeight = 10;
    public float lateralSpawnOffest = 4;

    private float lastSpawn;


    // Use this for initialization
    private SubmarineMovement submarine;
    private MapInfo map;


    void Start () {
        lastSpawn = delayBeforeSpawns;
        if (Game.instance != null)
            Game.instance.OnGameStart += Init;
    }

    void Init()
    {
        MapInfo m = Game.instance.map;
        submarine = Game.instance.submarine;
        map = Game.instance.map;
        Game.instance.OnGameStart -= Init;
        return;    
    }
	
	// Update is called once per frame
	void Update () {

        if (lastSpawn <= 0)
        {
            Vector3 spawnPos = Vector3.zero;


            float leftRight = Random.Range(0.0f, 1.0f);
            if (leftRight > 0.5f)
                spawnPos.x = lateralSpawnOffest;
            else
                spawnPos.x = -lateralSpawnOffest;

            float y = submarine.transform.position.y;

            spawnPos.y = +Random.Range(y - spawnAreaHeight / 2, (y + spawnAreaHeight / 2).Capped(map.mapTop));

            spawnPos.z = -0.2f;

            BaseFish newFish = map.DrawAtFishLottery(spawnPos.y);


            if( fishPool != null && newFish != null)
            {
                fishPool.SetFishAt(newFish, spawnPos);

            }
            /*
            if (newFish != null)
            {
                Instantiate(newFish.gameObject, spawnPos, Quaternion.identity);
            }
            */

            lastSpawn = map.GetGeneralDensity(spawnPos.y);
        }
        else lastSpawn -= Time.deltaTime;

    }
}
