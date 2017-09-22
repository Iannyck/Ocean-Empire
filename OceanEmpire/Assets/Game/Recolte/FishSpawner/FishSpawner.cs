using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    [System.Serializable]
    public struct FishType
    {
        BaseFish fish;

            //Fish repartition in map
        float maxDensityDepth;
        float highestDepth;
        float lowestDepth;
            //Fish denstity in zone
        float maximumDensity;
        float populationHealthEffect;
    }

    public List<FishType> fishTypeList;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
