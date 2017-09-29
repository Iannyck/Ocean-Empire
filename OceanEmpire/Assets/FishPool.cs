using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishPool : MonoBehaviour {
    Dictionary<FishDescription, Queue<BaseFish>> DesactivatedFishes = new Dictionary<FishDescription, Queue<BaseFish>>() ;

    public BaseFish SetFishAt(BaseFish fishPrefab, Vector2 spawnPos)
    {
        print("0");
        if (fishPrefab != null)
        {
            print("1");
            BaseFish fishInstance;
            try
            {
                fishInstance = DesactivatedFishes[fishPrefab.description].Dequeue();
                fishInstance.transform.position = spawnPos;
                fishInstance.transform.rotation = Quaternion.identity;
                fishInstance.gameObject.SetActive(true);
                print("2a");
            }
            catch
            {
                fishInstance = Instantiate(fishPrefab.gameObject, spawnPos, Quaternion.identity).GetComponent<BaseFish>();
                print("2b");
            }
            fishInstance.captureEvent += AddToPool;
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


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
