using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMeter : MonoBehaviour {

    private Text text;

	void Start ()
    {
        if (GetComponent<Text>() != null)
        {
            text = GetComponent<Text>();
            text.text = "";
        }
    }
	
	void Update ()
    {
        if (Game.Instance == null)
            return;
        if (!Game.Instance.gameStarted)
            return;
        if (GetComponent<Text>() != null)
            text.text = "00:" + (int)Game.PlayerStats.remainingTime;
    }
}
