using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour {

    public Text text;

    private string startDisplay;

	void Start ()
    {
        if(text != null)
            startDisplay = text.text;
    }
	
	void Update ()
    {
		if(PlayerCurrency.instance != null)
            text.text = startDisplay + PlayerCurrency.GetCoins() + "$";


        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerCurrency.AddCoins(1);
        }
	}
}
