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
            text.text = startDisplay + PlayerCurrency.Money + "$";

        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerCurrency.Add = 1;
        }
	}
}
