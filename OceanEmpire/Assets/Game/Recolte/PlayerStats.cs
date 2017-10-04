using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    private int score; // cash
    private float depthRecord;
    private List<BaseFish> fishInStock = new List<BaseFish>();

    public float remainingTimeAtStart = 30;
    [HideInInspector]
    public float remainingTime;

    void Start ()
    {
        score = 0;
        remainingTime = remainingTimeAtStart;
        // load depthRecord for this level
    }

    void Update()
    {
        if (Game.instance == null)
            return;
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0 && !Game.instance.gameOver)
            Game.instance.EndGame();
    }

    public void AddFishToStock(BaseFish fish)
    {
        fishInStock.Add(fish);
        // Add score
    }
}
