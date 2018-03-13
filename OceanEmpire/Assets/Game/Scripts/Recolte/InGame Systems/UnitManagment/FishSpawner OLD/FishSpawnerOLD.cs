//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class FishSpawnerOLD : MonoBehaviour
//{

//    bool Active = false;

//    [SerializeField] UnitInstantiator unitSpawner;

//    public float maxFishPerUnit = 1.5f;
//    public float palierHeigth = 1;
//    public float FishReplenishTime = 120;
//    public float timeBetweenSideSpawn = 10;

//    private int palierSpawnedOutsideCamera = 3;

//    private float lastSpawn;

//    private List<FishPalierOLD> listPaliers;

//    private float timeCounter;
//    struct PalierRange
//    {
//        public int start;
//        public int finish;
//    }

//    private PalierRange lastActivePaliers;

//    // Use this for initialization
//    private SubmarineMovement submarine;
//    private MapInfo map;
//    private GameCamera cam;

//    public FishPalierOLD GetPalier(int i)
//    {
//        int n = i.Clamped(0, listPaliers.Count - 1);
//        return listPaliers[n];
//    }

//    void Start()
//    {
//        lastSpawn = 0;
//        Game.OnGameStart += Init;
//    }

//    void Init()
//    {
//        submarine = Game.Instance.submarine;
//        map = Game.Instance.map;
//        cam = Game.GameCamera;

//        FishPalierOLD.repopulationCycle = 120;

//        StartPalierSystem();
//    }

//    void StartPalierSystem()
//    {
//        listPaliers = new List<FishPalierOLD>();
//        SetPalierFishLimit();

//        //PalierRange lastActivePaliers = new PalierRange
//        //{
//        //    start = GetClosestPalier(cam.Top + palierHeigth * palierSpawnedOutsideCamera),
//        //    finish = GetClosestPalier(cam.Bottom - palierHeigth * palierSpawnedOutsideCamera)
//        //};
//    }

//    public void SetPalierFishLimit()
//    {
//        int palierNumber = (int)Mathf.Ceil((map.mapTop - map.mapBottom) / palierHeigth);
//        float maxFishPerPalier = (maxFishPerUnit * palierHeigth);

//        for (int i = 0; i < palierNumber; i++)
//        {
//            FishPalierOLD newPalier = new FishPalierOLD();
//            listPaliers.Add(newPalier);

//            float fishLimit = maxFishPerPalier * map.GetGeneralDensity(GetPalierPosition(i));
//            newPalier.InitLimit(fishLimit);
//        }

//        Active = true;
//    }





//    public float GetPalierPosition(int index)
//    {
//        return map.mapTop - (index + 0.5f) * palierHeigth;
//    }

//    public int GetClosestPalier(float ypos)
//    {
//        ypos = ypos.Raised(map.mapBottom);
//        ypos = ypos.Capped(map.mapTop);
//        int palier = (int)Mathf.Round(((map.mapTop - ypos) + (palierHeigth / 2)) / palierHeigth) - 1;
//        return palier;
//    }




//    // Update is called once per frame
//    void Update()
//    {
//        if (!Active)
//            return;

//        SpawnSideFish();
//        UpdatePalier();

//        timeCounter += Time.deltaTime;
//    }


//    public void UpdatePalier()
//    {
//        PalierRange newPalier = new PalierRange
//        {
//            start = GetClosestPalier(cam.Top + palierHeigth * palierSpawnedOutsideCamera),
//            finish = GetClosestPalier(cam.Bottom - palierHeigth * palierSpawnedOutsideCamera)
//        };

//        //Spawn
//        while (newPalier.start < lastActivePaliers.start)
//        {
//            //start up
//            lastActivePaliers.start -= 1;
//            SpawnPalier(lastActivePaliers.start);
//        }
//        while (newPalier.finish > lastActivePaliers.finish)
//        {
//            //finish down
//            lastActivePaliers.finish += 1;
//            SpawnPalier(lastActivePaliers.finish);
//        }

//        //Despawn
//        while (newPalier.start > lastActivePaliers.start)
//        {
//            //start down
//            DespawnPalier(lastActivePaliers.start);
//            lastActivePaliers.start += 1;
//        }
//        while (newPalier.finish < lastActivePaliers.finish)
//        {
//            //finish up
//            DespawnPalier(lastActivePaliers.finish);
//            lastActivePaliers.finish -= 1;
//        }

//        lastActivePaliers = newPalier;
//    }

//    public void DespawnPalier(int i)
//    {
//        GetPalier(i).isActive = false;
//        GetPalier(i).Despawn(timeCounter);
//    }


//    public void SpawnPalier(int i)
//    {
//        FishPalierOLD p = GetPalier(i);
//        p.isActive = true;

//        float fishesToSpawn = p.GetFishDensity(timeCounter);

//        while (fishesToSpawn > 0)
//        {
//            if (fishesToSpawn < 1)
//            {
//                float spawnChances = Random.Range(0.0f, 1.0f);
//                if (spawnChances < fishesToSpawn)
//                    SpawnPalierFish(i);
//            }
//            else
//            {
//                SpawnPalierFish(i);
//            }
//            fishesToSpawn -= 1;
//        }
//    }

//    public void SpawnPalierFish(int palierIte)
//    {
//        Vector3 spawnPos = Vector3.zero;
//        float nb1 = Random.Range(-0.5f, 0.5f);
//        float nb2 = Random.Range(-0.5f, 0.5f);
//        float nb3 = Random.Range(cam.Left, cam.Right);

//        spawnPos.y = GetPalierPosition(palierIte) + (nb1 + nb2) * palierHeigth;
//        spawnPos.y = spawnPos.y.Clamped(map.mapBottom, map.mapTop);
//        spawnPos.x = nb3;

//        DrawAtLotteryAndSpawn(spawnPos);
//    }



//    void SpawnSideFish()
//    {
//        if (lastSpawn <= 0)
//        {
//            float spawnAreaHeight = cam.Height;
//            float lateralSpawnOffest = 1.2f * cam.HalfWidth;

//            Vector3 spawnPos = Vector3.zero;

//            float leftRight = Random.Range(0.0f, 1.0f);
//            if (leftRight > 0.5f)
//                spawnPos.x = lateralSpawnOffest;
//            else
//                spawnPos.x = -lateralSpawnOffest;
//            float y = submarine.transform.position.y;
//            spawnPos.y = +Random.Range(y - spawnAreaHeight / 2, (y + spawnAreaHeight / 2).Capped(map.mapTop));


//            FishPalierOLD closestPalier = GetPalier(GetClosestPalier(spawnPos.y));

//            if (closestPalier.GetFishDensity(timeCounter) > 1 || Random.Range(0.0f, 1.0f) > 0)
//            {
//                DrawAtLotteryAndSpawn(spawnPos);
//            }
//            lastSpawn = timeBetweenSideSpawn;
//        }

//        else lastSpawn -= Time.deltaTime;
//    }

//    private void DrawAtLotteryAndSpawn(Vector2 spawnPos)
//    {
//        var fish = map.DrawAtFishLottery(spawnPos.y);
//        if (fish == null)
//            return;

//        PendingFishGPC pending = Game.Instance.PendingFishGPC;

//        // Melee capture GPC
//        var meleeCapturable = fish.GetComponent<MeleeCapturable>();
//        if (meleeCapturable != null)
//        {
//            pending.AddPendingFishGPC(new GPComponents.GPC_MeleeCapture(Game.SceneManager, meleeCapturable, spawnPos));
//            return;
//        }

//        // Harpoon capture GPC
//        var harpoonCapturable = fish.GetComponent<HarpoonCapturable>();
//        if (harpoonCapturable != null)
//        {
//            pending.AddPendingFishGPC(new GPComponents.GPC_HarpoonCapture(Game.SceneManager, harpoonCapturable, spawnPos));
//            return;
//        }


//        // Not eligible for a GPC
//        Debug.LogWarning("Spawned a fish that was not elligible for any type of GPC");
//        unitSpawner.Spawn(fish, spawnPos);
//    }
//}
