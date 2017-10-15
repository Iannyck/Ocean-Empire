using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    bool Active = false;

    public FishPool fishPool;

    public float maxFishPerUnit = 1.5f;
    public float palierHeigth = 1;
    public float FishReplenishTime = 120;
    public float timeBetweenSideSpawn = 10;

    private int palierSpawnedOutsideCamera = 3;

    private float lastSpawn;

    private List<FishPalier> listPaliers;

    private float timeCounter;
    private int fishCount = 0;
    struct palierRange
    {
        public int start;
        public int finish;
    }

    private palierRange lastActivePaliers;
   

    // Use this for initialization
    private SubmarineMovement submarine;
    private MapInfo map;
    private GameCamera cam;

    public FishPalier GetPalier(int i)
    {
        int n = i.Clamped(0, listPaliers.Count - 1);
        return listPaliers[n];
    }

    void Start () {
        lastSpawn = 0;
        Game.OnGameStart += Init;
    }

    void Init()
    {
        Active = true;

        MapInfo m = Game.instance.map;
        submarine = Game.instance.submarine;
        map = Game.instance.map;
        Game.OnGameStart -= Init;
        cam = Game.GameCamera;

        FishPalier.repopulationCycle = 120;

        StartPalierSystem();
        return;    
    }



    void StartPalierSystem()
    {
        listPaliers = new List<FishPalier>();
        int palierNumber = (int)Mathf.Ceil((map.mapTop - map.mapBottom) / palierHeigth);

        float maxFishPerPalier = (maxFishPerUnit * palierHeigth);
        

        for (int i = 0; i < palierNumber; i++)
        {
            FishPalier newPalier = new FishPalier();
            listPaliers.Add(newPalier);

            float fishLimit = maxFishPerPalier * map.GetGeneralDensity(GetPalierPosition(i));
            newPalier.Init(fishLimit);
        }


        palierRange lastActivePaliers = new palierRange();

        lastActivePaliers.start = GetClosestPalier(cam.Top + palierHeigth * palierSpawnedOutsideCamera);
        lastActivePaliers.finish = GetClosestPalier(cam.Bottom - palierHeigth * palierSpawnedOutsideCamera);
        /*
        for (int i = lastActivePaliers.start; i <= lastActivePaliers.finish; i++)
        {

            SpawnPalier(i);
        }
      */
    }



    public float GetPalierPosition(int iterator)
    {
        return map.mapTop - (iterator + 0.5f) * palierHeigth;
    }

    public int GetClosestPalier(float ypos)
    {
        ypos = ypos.Raised(map.mapBottom);
        ypos = ypos.Capped(map.mapTop);
        int palier = (int)Mathf.Round(( (map.mapTop - ypos) + (palierHeigth/2) ) / palierHeigth);
        return palier;
    }




    // Update is called once per frame
    void Update () {
        if (!Active)
            return;

        SpawnSideFish();
        UpdatePalier();

        timeCounter += Time.deltaTime;     
    }


    void UpdatePalier()
    {
        palierRange newPalier = new palierRange();

        newPalier.start = GetClosestPalier(cam.Top + palierHeigth * palierSpawnedOutsideCamera);
        newPalier.finish = GetClosestPalier(cam.Bottom - palierHeigth * palierSpawnedOutsideCamera);

        //Spawn
        while (newPalier.start < lastActivePaliers.start)
        {
           //start up
            lastActivePaliers.start -= 1;
            SpawnPalier(lastActivePaliers.start);         
        }
        while (newPalier.finish > lastActivePaliers.finish)
        {
            //finish down
            lastActivePaliers.finish += 1;
            SpawnPalier(lastActivePaliers.finish);
        }

        //Despawn
        while (newPalier.start > lastActivePaliers.start)
        {
            //start down
            DespawnPalier(lastActivePaliers.start);
            lastActivePaliers.start += 1;
        }
        while (newPalier.finish < lastActivePaliers.finish)
        {
            //finish up
            DespawnPalier(lastActivePaliers.finish);
            lastActivePaliers.finish -= 1;
        }

        lastActivePaliers = newPalier;
    }

    public void DespawnPalier(int i)
    {
        GetPalier(i).isActive = false;
        GetPalier(i).Despawn(timeCounter);
    }


    public void SpawnPalier(int i)
    {
        FishPalier p = GetPalier(i);
        p.isActive = true;

        float fishesToSpawn = p.GetFishDensity(timeCounter);

        while (fishesToSpawn > 0)
        {
            if (fishesToSpawn < 1) {
                float spawnChances = Random.Range(0.0f, 1.0f);
                if (spawnChances < fishesToSpawn)
                    SpawnPalierFish(i);
            }
            else {
                SpawnPalierFish(i);
            }
            fishesToSpawn -= 1;
        }
    }

    public void SpawnPalierFish(int palierIte)
    {
        fishCount += 1;

        Vector3 spawnPos = Vector3.zero;
        float nb1 = Random.Range(-0.5f, 0.5f);
        float nb2 = Random.Range(-0.5f, 0.5f);
        float nb3 = Random.Range(cam.Left, cam.Right);

        spawnPos.y = GetPalierPosition(palierIte) + (nb1 + nb2) * palierHeigth;
        spawnPos.y = spawnPos.y.Clamped(map.mapBottom, map.mapTop);
        spawnPos.x = nb3;

        BaseFish newFish = map.DrawAtFishLottery(spawnPos.y);
        if (fishPool != null && newFish != null)
        {
            BaseFish someFish = fishPool.SetFishAt(newFish, spawnPos);
            //GetPalier(palierIte).SuscribeFish(someFish);
        }
    }





    void SpawnSideFish()
    {
        fishCount += 1;
        if (lastSpawn <= 0)
        {
            float spawnAreaHeight = cam.Height;
            float lateralSpawnOffest = 1.2f * cam.HalfWidth;


            Vector3 spawnPos = Vector3.zero;

            Random.InitState(fishCount);
            float leftRight = Random.Range(0.0f, 1.0f);
            if (leftRight > 0.5f)
                spawnPos.x = lateralSpawnOffest;
            else
                spawnPos.x = -lateralSpawnOffest;

            float y = submarine.transform.position.y;

            spawnPos.y = +Random.Range(y - spawnAreaHeight / 2, (y + spawnAreaHeight / 2).Capped(map.mapTop));

            BaseFish newFish = map.DrawAtFishLottery(spawnPos.y);

            if (fishPool != null && newFish != null)
            {
                fishPool.SetFishAt(newFish, spawnPos);
            }
            lastSpawn = map.GetGeneralDensity(spawnPos.y) * timeBetweenSideSpawn;

        }
        else lastSpawn -= Time.deltaTime;
    }
}
