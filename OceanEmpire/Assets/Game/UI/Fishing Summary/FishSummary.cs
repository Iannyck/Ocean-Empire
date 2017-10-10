using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishSummary : MonoBehaviour {

    public Text amount;
    public string baseText;
    public Image icon;
    public Text poissonName;

	public void SetFishSummary(int amount, Sprite iconImage, string poissonName)
    {
        this.amount.text = baseText + amount;
        icon.sprite = iconImage;
        this.poissonName.text = poissonName;
    }
}
