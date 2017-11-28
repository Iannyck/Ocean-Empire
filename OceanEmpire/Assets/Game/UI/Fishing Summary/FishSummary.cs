using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishSummary : MonoBehaviour {

    public Image fishIcon;
    public Text fishName;
    public Text fishValue;
    public Text amount;

    private int fishAmount;

    [SerializeField, ReadOnly]
    public FishDescription description;

    public void SetFishSummary(FishDescription desc, int amount)
    {
        description = desc;
        fishIcon.sprite = desc.icon.GetSprite();
        fishName.text = desc.name;
        fishValue.text = desc.baseMonetaryValue.ToString();

        fishAmount = amount;
        this.amount.text = fishAmount.ToString();
    }

    public void AddFish()
    {
        fishAmount++;
        this.amount.text = fishAmount.ToString();
    }
}
