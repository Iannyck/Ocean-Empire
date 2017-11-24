using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantExerciseChoice_Item : MonoBehaviour
{
    [Header("UI Links")]
    public Text taskText;

    [Header("Coin display")]
    public GameObject coinDisplay;
    public Text coinText;

    [Header("Tickets display")]
    public GameObject ticketDisplay;
    public Text ticketText;

    [Header("Ocean Refill display")]
    public GameObject oceanRefillDisplay;

    public delegate void Event(InstantExerciseChoice_Item item);
    public Task assignedTask;
    public Event onClick;

    public void DisplayTask(Task task)
    {
        assignedTask = task;

        DisableRewardDisplays();

        Reward reward = task.GetReward();
        switch (reward.GetRewardType())
        {
            case RewardType.Coins:
                coinDisplay.SetActive(true);
                break;
            case RewardType.Tickets:
                ticketDisplay.SetActive(true);
                break;
            case RewardType.OceanRefill:
                oceanRefillDisplay.SetActive(true);
                break;
        }

        //TEMPORAIRE
        taskText.text = "Marche de " + (task as WalkTask).minutesOfWalk + " min";
    }

    private void DisableRewardDisplays()
    {
        coinDisplay.SetActive(false);
        ticketDisplay.SetActive(false);
        oceanRefillDisplay.SetActive(false);
    }

    public void Click()
    {
        if (onClick != null)
            onClick(this);
    }
}
