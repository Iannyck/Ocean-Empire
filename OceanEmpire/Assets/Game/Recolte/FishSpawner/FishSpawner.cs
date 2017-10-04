using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    bool Active = false;

    public FishPool fishPool;

    public float delayBeforeSpawns = 2;
    public float spawnAreaHeight = 10;
    public float lateralSpawnOffest = 4;
    public float maxFishPerUnit = 1.5f;
    public float palierHeigth = 1;

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
        return listPaliers[i];
    }

    void Start () {
        lastSpawn = delayBeforeSpawns;
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
        lastActivePaliers.start = GetClosestPalier(cam.Top + palierHeigth);
        lastActivePaliers.finish = GetClosestPalier(cam.Bottom - palierHeigth);

        print("start: " + lastActivePaliers.start + " finish: " + lastActivePaliers.finish);

        
        for (int i = lastActivePaliers.start; i <= lastActivePaliers.finish; i++)
        {

            SpawnPalier(i);
        }
      
    }



    public float GetPalierPosition(int iterator)
    {
        return map.mapTop - (iterator + 0.5f) * palierHeigth;
    }

    public int GetClosestPalier(float ypos)
    {
        ypos = ypos.Raised(map.mapBottom);
        ypos = ypos.Capped(map.mapTop);
        int palier = (int)Mathf.Ceil((map.mapTop - ypos) / palierHeigth);
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

        newPalier.start = GetClosestPalier(cam.Top + palierHeigth);
        newPalier.finish = GetClosestPalier(cam.Bottom - palierHeigth);

        //Spawn
        while (newPalier.start < lastActivePaliers.start)
        {
            SpawnPalier(newPalier.start);
            lastActivePaliers.start -= 1;
        }
        while (newPalier.finish > lastActivePaliers.finish)
        {
            SpawnPalier(newPalier.finish);
            lastActivePaliers.finish += 1;
        }

        //Despawn
        while (newPalier.start > lastActivePaliers.start)
        {
            DespawnPalier(newPalier.start);
            lastActivePaliers.start += 1;
        }
        while (newPalier.finish < lastActivePaliers.finish)
        {
            DespawnPalier(newPalier.finish);
            lastActivePaliers.finish -= 1;
        }
    }



    void SpawnSideFish()
    {
        fishCount += 1;
        if (lastSpawn <= 0)
        {
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
            lastSpawn = map.GetGeneralDensity(spawnPos.y);
            
        }
        else lastSpawn -= Time.deltaTime;
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
        spawnPos.x = nb3;

        BaseFish newFish = map.DrawAtFishLottery(spawnPos.y);
        if (fishPool != null && newFish != null)
        {
            BaseFish someFish = fishPool.SetFishAt(newFish, spawnPos);
            GetPalier(palierIte).SuscribeFish(someFish);
        }
    }




    public void DespawnPalier(int i)
    {
        GetPalier(i).Despawn(timeCounter);
        GetPalier(i).isActive = false;
    }

}
