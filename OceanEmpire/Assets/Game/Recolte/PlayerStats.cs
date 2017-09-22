using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public int score; // cash
    public float depthRecord;
    public List<BaseFish> fishInStock = new List<BaseFish>();

    void Start ()
    {
        score = 0;
        // load depthRecord for this level
    }

    public void AddFishToStock(BaseFish fish)
    {
        fishInStock.Add(fish);
        // Add score
    }
}
