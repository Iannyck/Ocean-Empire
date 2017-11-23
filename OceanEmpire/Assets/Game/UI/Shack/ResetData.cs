using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetData : MonoBehaviour {

	public void ResetDataToDefault()
    {
        if (ItemsList.instance != null)
            ItemsList.ResetToDefaults();

        if (PlayerCurrency.instance != null)
            PlayerCurrency.ResetToDefaults();

        if(FishPopulation.instance != null)
            FishPopulation.ResetToDefaults();
    }
    public void ShowMeTheMoney()    //Cheat code StarCraft xD
    {
        if (PlayerCurrency.instance != null)
        {
            PlayerCurrency.AddCoins(1000);
        }           
    }
}
