using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    //private int score; // cash
    private float depthRecord;

    public bool infiniteTime = false;
    public float remainingTimeAtStart = 30;
    [ReadOnly]
    public float remainingTime;

    void Start ()
    {
        //score = 0;
        remainingTime = remainingTimeAtStart;
        // load depthRecord for this level
    }

    void Update()
    {
        if (Game.instance == null)
            return;

        if (!infiniteTime && Game.instance.gameStarted)
            UpdateTimer();
    }

    void UpdateTimer()
    {
        remainingTime = (remainingTime - Time.deltaTime).Raised(0);

        if (remainingTime <= 0 && !Game.instance.gameOver)
            Game.instance.EndGame();
    }

    public void TryCapture(BaseFish fish)
    {
        FishContainer container = Game.SubmarinParts.GetFishContainer();
        if ( container.HasRoom())
        {
            fish.Capture();
            container.AddFish(fish);

            CallCoinsPopUp(fish);
        }
    }

    private void CallCoinsPopUp(BaseFish fish)
    {
        int fishWorth = fish.description.baseMonetaryValue.RoundedToInt();
        Vector3 fishPostion = fish.transform.position;

        Game.Recolte_UI.GetComponent<SpawnCoinsPopUp>().SpawnPopUp(fishPostion, fishWorth);
    }
}
