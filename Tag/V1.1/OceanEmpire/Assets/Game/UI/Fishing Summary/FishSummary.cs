using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishSummary : MonoBehaviour {

    public Text amount;
    public Image icon;
    public Text reward;

	public void SetFishSummary(int amount, Sprite iconImage, string reward)
    {
        this.amount.text = amount + "x";
        icon.sprite = iconImage;
        this.reward.text = reward;
    }
}
