using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    //private int score; // cash
    private float depthRecord;

    public bool infiniteTime = false;
    public float remainingTimeAtStart = 30;
    [ReadOnly]
    public float remainingTime;

    void Start()
    {
        //score = 0;
        remainingTime = remainingTimeAtStart;
        // load depthRecord for this level
    }

    void Update()
    {
        if (Game.Instance == null)
            return;

        if (!infiniteTime && Game.Instance.gameStarted)
            UpdateTimer();
    }

    void UpdateTimer()
    {
        remainingTime = (remainingTime - Time.deltaTime).Raised(0);

        if (remainingTime <= 0 && !Game.Instance.gameOver)
            Game.Instance.EndGame();
    }

    public void TryCapture(Capturable fish)
    {
        FishContainer container = Game.SubmarinParts.GetFishContainer();
        if (container.HasRoom())
        {
            fish.Capture();
            container.AddFish(fish.info);

            CallCoinsPopUp(fish.info);
        }
    }

    private void CallCoinsPopUp(FishInfo info)
    {
        int fishWorth = info.description.baseMonetaryValue.RoundedToInt();
        Vector3 fishPostion = info.transform.position;

        Game.Recolte_UI.GetComponent<SpawnCoinsPopUp>().SpawnPopUp(fishPostion, fishWorth);
    }
}
