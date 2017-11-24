using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour {
    Dictionary<FishDescription, Queue<BaseFish>> DesactivatedFishes = new Dictionary<FishDescription, Queue<BaseFish>>() ;

    public BaseFish SetFishAt(BaseFish fishPrefab, Vector3 spawnPos)
    {
        if (fishPrefab != null)
        {
            BaseFish fishInstance;
            try
            {
                fishInstance = DesactivatedFishes[fishPrefab.description].Dequeue();
                fishInstance.transform.position = spawnPos;
                fishInstance.transform.rotation = Quaternion.identity;
                fishInstance.gameObject.SetActive(true);
            }
            catch
            {
                fishInstance = Game.Spawner.Spawn(fishPrefab, spawnPos);
            }
            fishInstance.deathEvent += AddToPool;

            Game.FishingReport.KeepTrack(fishInstance);

            fishInstance.RemiseEnLiberté();
       

            return fishInstance;
        }
        else return null;
    }

    private void AddToPool(BaseFish fish)
    {
        if (!DesactivatedFishes.ContainsKey(fish.description))
            DesactivatedFishes.Add(fish.description, new Queue<BaseFish>());

        DesactivatedFishes[fish.description].Enqueue(fish);
    }
}
