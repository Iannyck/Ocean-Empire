using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ocean Empire/Item/Fish Container")]
public class FishContainer : Upgrade {


    public float fishCapacity = 20;
    [SerializeField, ReadOnly]
    private float fishContained = 0;

    public float GetFishCapacity()
    {
        return fishCapacity;
    }

    public bool HasRoom()
    {
        return (fishContained < fishCapacity);
    }

    public void AddFish(BaseFish fish)
    { 
        fishContained += fish.description.populationValue;   
    }

    public void ResetContainedFish()
    {
        fishContained = 0;
    }
}
