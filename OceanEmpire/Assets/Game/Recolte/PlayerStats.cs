using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private int score; // cash
    private float depthRecord;
    private List<BaseFish> fishInStock = new List<BaseFish>();

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
