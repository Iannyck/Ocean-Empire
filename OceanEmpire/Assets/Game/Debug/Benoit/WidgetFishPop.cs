using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WidgetFishPop : MonoBehaviour {

    public Slider gageMeter;

	// Update is called once per frame
	void Update () {

        FishPopulation.instance.RefreshPopulation();
        gageMeter.value = FishPopulation.PopulationRate;

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            print("mad");
            FishPopulation.instance.AddRate(0.2f);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        { 
            FishPopulation.instance.AddRate(-0.2f);
        }
    }

}
