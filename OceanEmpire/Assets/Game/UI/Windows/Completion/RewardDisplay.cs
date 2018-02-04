using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardDisplay : MonoBehaviour {

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text amountDisplay;

	public void UpdateRewardInfo(int amount, Sprite icon)
    {
        this.icon.sprite = icon;
        amountDisplay.text = "X" + amount;
    }
}
